using GK_Projekt4.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    class Triangle
    {
        class VertexData
        {
            public VertexData()
            {
            }

            public VertexData(double screenX, int screenY, double screenZ, 
                double clippingW, double[] worldCoords, 
                double[] normal, double[] binormal, double[] tangent, PointF textureCoords)
            {
                X = screenX;
                Y = screenY;
                Z = screenZ;
                WorldCoords = worldCoords;
                Wc = clippingW;
                Normal = normal;
                Binormal = binormal;
                Tangent = tangent;
                TextureCoords = textureCoords;
            }

            public double[] WorldCoords { get; set; }
            public double[] Normal { get; set; }
            public double[] Binormal { get; set; }
            public double[] Tangent { get; set; }
            public double Z { get; set; }
            public double Wc { get; set; }  //współrzędna 'w' wektora [x, y, z, w] w przestrzeni obcinania - potrzebna przy korekcji perspektywy
            public double X { get; set; }  //uwaga, ten X specjalnie jest double, bo przy konstruowaniu punktu przecięcia intersection w algorytmie scanlinii dla trójkątów X może wyjść np. Inifnity i dla mnie jest to ważne
            public int Y { get; set; }
            public PointF TextureCoords { get; set; }
        }

        readonly double[][] _clippingCoords;
        readonly double[][] _worldCoords;
        readonly double[][] _normals;
        readonly double[][] _binormals;
        readonly double[][] _tangents;
        readonly PointF[] _textureCoords;
        readonly Figure _figure;

        //Współrzędne w przestrzeni świata i odpowiednie wektory będą potrzebne
        //przy oświetleniu
        public Triangle(double[][] clippingCoords, double[][] worldCoords, 
            double[][] normals, double[][] binormals, double[][] tangents, PointF[] textureCoords,
            Figure figure)
        {
            _clippingCoords = clippingCoords;
            _worldCoords = worldCoords;
            _normals = normals;
            _binormals = binormals;
            _tangents = tangents;
            _textureCoords = textureCoords;
            _figure = figure;
        }

        private void DrawBorders(DirectBitmap bitmap, bool backfaceCulling, double[,] zBuffer)
        {
            VertexData[] vertices = GetVertexDatas(bitmap.Width, bitmap.Height);
            if (backfaceCulling && IsAnticlockwise(vertices))
                return;
            for (int i = 0; i < 3; i++)
            {
                Bresenham.MidpointLine(bitmap,
                    (int)(vertices[i].X), vertices[i].Y, vertices[i].Z,
                    (int)(vertices[(i + 1) % 3].X), vertices[(i+1)%3].Y, vertices[(i+1)%3].Z,
                    zBuffer, _figure.Kd);
            }
        }

        private static bool IsAnticlockwise(VertexData[] vertices)
        {
            return vertices[0].X * vertices[1].Y + vertices[1].X * vertices[2].Y
                + vertices[2].X * vertices[0].Y - vertices[0].Y * vertices[1].X
                - vertices[1].Y * vertices[2].X - vertices[2].Y * vertices[0].X >= 0;
        }

        private VertexData[] GetVertexDatas(int width, int height)
        {
            var result = new VertexData[3];
            for (int i = 0; i < 3; i++)
            {
                var ndc = Utils.NormalizeToNDC(_clippingCoords[i]);
                result[i] = new VertexData(
                    Math.Max(0, Math.Min(width - 1, (int)((ndc[0] + 1d) * width / 2d))),  //tutaj jest przetwarzanie na współrzędne ekranu
                    Math.Max(0, Math.Min(height - 1, (int)((-ndc[1] + 1d) * height / 2d))),  
                    (ndc[2] + 1d) / 2d,
                    _clippingCoords[i][3], _worldCoords[i], _normals[i], _binormals[i],
                    _tangents[i], _textureCoords[i]);
            }
            return result;
        }

        public void Draw(DirectBitmap bitmap, bool backfaceCulling, double[,] zBuffer,
            bool perspectiveCorrectiom, List<Light> lights, double[] cameraCoords)
        {
            switch (_figure.Fill)
            {
                case Figure.FillType.None:
                    DrawBorders(bitmap, backfaceCulling, zBuffer);
                    break;
                case Figure.FillType.SingleColor:
                case Figure.FillType.Texture:
                    FillTriangle(bitmap, backfaceCulling, zBuffer, perspectiveCorrectiom,
                        lights, cameraCoords);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Niepoprawna wartość enuma");
            }
        }

        //Algorytm scanlinii zoptymalizowany pod trójkąty
        private void FillTriangle(DirectBitmap bitmap,
            bool backfaceCulling, double[,] zBuffer, 
            bool perspectiveCorrection,
            List<Light> lights, double[] cameraCoords)
        {
            VertexData[] vertices = GetVertexDatas(bitmap.Width, bitmap.Height);
            if (backfaceCulling && IsAnticlockwise(vertices))
                return;

            Array.Sort(vertices, (v1, v2) =>
            {
                var result = v1.Y.CompareTo(v2.Y);
                if (result == 0)
                    return v1.X.CompareTo(v2.X);
                return result;
            });

            //algorytm scanlinii zoptymalizowany dla trójkątów, czyli 
            //wyznaczam punkt przecięcia poziomej prostej przechodzącej przez
            //wierzchołek vertices[1] (vertices są posortowane po Y)
            //oraz odcinka vertices[0] - vertices[2]
            //Argument perspectiveCorrection jest potrzebny w wyliczaniu
            //przecięcia, by odpowiednio zinterpolować wektory
            VertexData intersection = 
                ComputeIntersection(vertices, perspectiveCorrection);
            if (intersection == null)
                return;

            VertexData left, right;
            if (intersection.X < vertices[1].X)
            {
                left = intersection;
                right = vertices[1];
            }
            else
            {
                left = vertices[1];
                right = intersection;
            }

            IVerticalInterpolationData leftVerticalData, rightVerticalData;
            leftVerticalData = GetVerticalInterpolationData(left, vertices[2], lights, cameraCoords,
                _figure, perspectiveCorrection);
            rightVerticalData = GetVerticalInterpolationData(right, vertices[2], lights, cameraCoords,
                _figure, perspectiveCorrection);

            while (!leftVerticalData.HasFinished)
            {
                var horizontalData = leftVerticalData.GetHorizontalInterpolationData(rightVerticalData);
                while (!horizontalData.HasFinished)
                {
                    if (zBuffer == null || horizontalData.Z <=
                        zBuffer[horizontalData.X, horizontalData.Y])
                    {
                        bitmap.SetPixel(horizontalData.X, horizontalData.Y,
                            horizontalData.CalculateColor());
                        if (zBuffer != null)
                            zBuffer[horizontalData.X, horizontalData.Y] = horizontalData.Z;
                    }
                    horizontalData.Increment();
                };
                leftVerticalData.Increment();
                rightVerticalData.Increment();
            };

            leftVerticalData = GetVerticalInterpolationData(left, vertices[0], lights, cameraCoords,
               _figure, perspectiveCorrection);
            rightVerticalData = GetVerticalInterpolationData(right, vertices[0], lights, cameraCoords,
                _figure, perspectiveCorrection);

            //Na początku inkrementuję, bo poziomą scanlinię na wysokości przecięcia (intersection) przerobiłem już
            //w poprzedniej pętli
            leftVerticalData.Increment();
            rightVerticalData.Increment();
            while (!leftVerticalData.HasFinished)
            {
                var horizontalData = leftVerticalData.GetHorizontalInterpolationData(rightVerticalData);
                while (!horizontalData.HasFinished)
                {
                    if (zBuffer == null || horizontalData.Z <=
                        zBuffer[horizontalData.X, horizontalData.Y])
                    {
                        bitmap.SetPixel(horizontalData.X, horizontalData.Y,
                            horizontalData.CalculateColor());
                        if (zBuffer != null)
                            zBuffer[horizontalData.X, horizontalData.Y] = horizontalData.Z;
                    }
                    horizontalData.Increment();
                };
                leftVerticalData.Increment();
                rightVerticalData.Increment();
            };

        }

        IVerticalInterpolationData GetVerticalInterpolationData(
            VertexData from, VertexData to, List<Light> lights, double[] cameraCoords, 
            Figure _figure, bool perspectiveCorrection)
        {
            if (lights == null)
            {
                if (_figure.Fill == Figure.FillType.SingleColor)
                    return new VerticalBasicInterpolation(
                        from, to, _figure, perspectiveCorrection);
                else
                    return new VerticalTextureInterpolation(
                        from, to, _figure, perspectiveCorrection);
            }
            else
            {
                if (_figure.Fill == Figure.FillType.SingleColor)
                    return new VerticalPhongInterpolation(
                        from, to, lights, cameraCoords, _figure, perspectiveCorrection);
                else
                    return new VerticalTexturePhongInterpolation(
                        from, to, lights, cameraCoords, _figure, perspectiveCorrection);
            }
        }

        private VertexData ComputeIntersection(VertexData[] vertices,
            bool perspectiveCorrection)
        {
            //Niech p1, p2, p3 oznaczają posortowane punkty
            //Wyznaczam teraz punkt przecięcia prostej przechodzącej przez
            //p2 równoległej do osi poziomej z prostą p1p3
            //Wzór wyliczyłem ręcznie na kartce papieru, działa też w przypadku,
            //gdy p1p3 jest pionowe, bo wtedy odpowiednio zachowują się double.Infinity itp.
            double tmp = 1d / (vertices[0].Y - vertices[2].Y);
            var intersectionX = (vertices[0].X
                + (vertices[1].Y - vertices[0].Y)
                * (vertices[0].X - vertices[2].X)
                * tmp);
            if (double.IsNaN(intersectionX))   //to jest dla sytuacji, gdy np. mamy punkty (0,233), (0,233), (4,233)
                return null;
            var intersectionY = vertices[1].Y;
            //Dla punktu przecięcia wyznaczam też zinterpolowaną współrzędną Z
            var intersectionZ = vertices[0].Z
                + (vertices[1].Y - vertices[0].Y)
                * (vertices[0].Z - vertices[2].Z)
                * tmp;
            ILinearIntepolation linearIntepolation;
            if (perspectiveCorrection)
                linearIntepolation = new LinearIntepolationWithPC(vertices[0].Wc,
                    vertices[2].Wc, vertices[2].Y - vertices[0].Y);
            else
                linearIntepolation = new LinearInterpolation(vertices[2].Y
                    - vertices[0].Y);
            //Nie ma obaw przy rzutowaniu na uinta tutaj, bo intersection.Y >= vertices[0].Y
            linearIntepolation.IncrementDistance((uint)(intersectionY - vertices[0].Y));

            return new VertexData(intersectionX, intersectionY, intersectionZ,
                linearIntepolation.Interpolate(vertices[0].Wc, vertices[2].Wc),
                linearIntepolation.Interpolate(vertices[0].WorldCoords, vertices[2].WorldCoords),
                linearIntepolation.InterpolateVector(vertices[0].Normal, vertices[2].Normal),
                linearIntepolation.InterpolateVector(vertices[0].Binormal, vertices[2].Binormal),
                linearIntepolation.InterpolateVector(vertices[0].Tangent, vertices[2].Tangent),
                linearIntepolation.Interpolate(vertices[0].TextureCoords, vertices[2].TextureCoords)
                );
        }

        class PointAttributes
        {
            public PointAttributes() { }

            public PointAttributes(double wc, double[] worldCoords, double[] normal, 
                double[] binormal, double[] tangent, PointF textureCoords)
            {
                Wc = wc;
                WorldCoords = worldCoords;
                Normal = normal;
                Binormal = binormal;
                Tangent = tangent;
                TextureCoords = textureCoords;
            }

            public PointAttributes Clone()
            {
                double[] worldCoords=null, normal=null, binormal=null, tangent=null;
                if (WorldCoords != null)
                {
                    worldCoords = new double[3];
                    Array.Copy(WorldCoords, worldCoords, 3);
                }
                if (Normal != null)
                {
                    normal = new double[3];
                    Array.Copy(Normal, normal, 3);
                }
                if (Binormal != null)
                {
                    binormal = new double[3];
                    Array.Copy(Binormal, binormal, 3);
                }
                if (Tangent != null)
                {
                    tangent = new double[3];
                    Array.Copy(Tangent, tangent, 3);
                }

                return new PointAttributes(Wc, worldCoords, normal, binormal, tangent, TextureCoords);
            }

            public double Wc { get; set; }
            public double[] WorldCoords { get; set; }
            public double[] Normal { get; set; }
            public double[] Binormal { get; set; }
            public double[] Tangent { get; set; }
            public PointF TextureCoords { get; set; }
        }

        //Ogólny interfejs
        interface IInterpolationData
        {
            int X { get;}
            int Y { get; }
            double Z { get; }
            bool HasFinished { get; }
            void Increment();
        }
        //Interfejs na obiekt służący interpolacji poruszając się w pionie (czyli na brzegu trójkąta)
        interface IVerticalInterpolationData: IInterpolationData
        {
            IHorizontalInterpolationData GetHorizontalInterpolationData(IVerticalInterpolationData right);
            PointAttributes CurrentAttributes { get; }
        }
        //Interfejs na obiekt służący interpolacji poruszając się w poziomie (czyli właściwa skanlinia)
        interface IHorizontalInterpolationData: IInterpolationData
        {
             Color CalculateColor();
        }

        //Klasa abstrakcyjna na obiekt służący interpolacji w pionie (w sensie po brzegu trójkąta i inkrementując/dekrementując y)
        abstract class VerticalInterpolation: IVerticalInterpolationData
        {
            double x;
            readonly double xIncr;
            readonly protected int y1, y2;
            readonly int yIncr;
            readonly double zIncr;
            readonly protected Figure figure;
            readonly protected ILinearIntepolation linearInterpolation;  //obiekt służący do interpolacji innych atrybutów niż współrzędna z głębokości, on odpowiada za korekcję perspektywy
            readonly protected bool perspectiveCorrection;
            readonly protected PointAttributes attributes1;   //atrybuty punktu, z którego zaczynamy
            readonly protected PointAttributes attributes2;  //atrybuty punktu docelowego

            public VerticalInterpolation(VertexData from, VertexData to, 
                Figure figure, bool perspectiveCorrection)
            {
                this.perspectiveCorrection = perspectiveCorrection;

                double tmp = 1d / Math.Abs(to.Y - from.Y);
                xIncr = (to.X - from.X) * tmp;
                zIncr = (to.Z - from.Z) * tmp;

                y1 = from.Y;
                y2 = to.Y;
                x = from.X + 0.5d;   //dodaję 0.5d, gdyż (int)(x+0.5d) = Math.Round(x), czyli będę później zaokrąglał jedynie rzutowaniem do int
                Y = from.Y;
                Z = from.Z;
                yIncr = from.Y <= to.Y ? 1 : -1;

                this.figure = figure;

                if (perspectiveCorrection)
                    linearInterpolation = new LinearIntepolationWithPC(
                        from.Wc, to.Wc, to.Y - from.Y);
                else
                    linearInterpolation = new LinearInterpolation(
                        to.Y - from.Y);

                attributes1 = new PointAttributes(from.Wc, from.WorldCoords,
                    from.Normal, from.Binormal, from.Tangent, from.TextureCoords);
                attributes2 = new PointAttributes(to.Wc, to.WorldCoords,
                    to.Normal, to.Binormal, to.Tangent, to.TextureCoords);
                CurrentAttributes = attributes1.Clone();  //na początku aktualne atrybuty są takie same, jak atrybuty punktu, z którego wychodzimy
            }
            public int X => (int)x;
            public int Y { get; private set; }
            public double Z { get; private set; }
            public PointAttributes CurrentAttributes { get; } //aktualne atrybuty w danym pikselu (do interpolacji właśnie tych atrybutów będę stosować linearIntepolation)
            public virtual void Increment()
            {
                Y += yIncr;
                x += xIncr;
                Z += zIncr;

                linearInterpolation.IncrementDistance();   //Tutaj odpowiednio zmieniamy współczynniki do interpolacji liniowej

                if ((yIncr > 0 && Y > y2) ||
                        (yIncr < 0 && Y < y2))
                    HasFinished = true;
            }

            public abstract IHorizontalInterpolationData GetHorizontalInterpolationData(
                IVerticalInterpolationData right);

            public bool HasFinished { get; private set; } = false;

            //Klasa abstrakcyjna do interpolowania w poziomie (czyli konkretna skanlinia)
            protected abstract class HorizontalInterpolation : 
                IHorizontalInterpolationData
            {
                readonly double zIncr;
                readonly protected int x1, x2;
                protected ILinearIntepolation linearInterpolation;
                readonly protected PointAttributes attributes1;  //atrybuty punktu, z którego zaczynamy (lewy kraniec scanlinii)
                readonly protected PointAttributes attributes2;  //atrubuty punktu docelowego (prawy kraniec scanlinii)
                readonly protected PointAttributes currentAttributes;  //atrybuty aktualnego punktu, w którym jesteśmy
                protected Figure figure;

                public HorizontalInterpolation(int left, int right, int y,
                    double leftZ, double rightZ, PointAttributes leftAttributes,
                    PointAttributes rightAttributes, Figure figure,
                    bool perspectiveCorrection)
                {
                    this.figure = figure;

                    x1 = left;
                    x2 = right;
                    X = left;
                    Y = y;
                    Z = leftZ;
                    zIncr = (rightZ - leftZ) / (right - left);

                    if (perspectiveCorrection)
                        linearInterpolation = new LinearIntepolationWithPC(
                            leftAttributes.Wc, rightAttributes.Wc, right - left);
                    else
                        linearInterpolation = new LinearInterpolation(
                            right - left);

                    attributes1 = leftAttributes;
                    attributes2 = rightAttributes;
                    currentAttributes = leftAttributes.Clone();
                }
                public int X { get; private set; }
                public int Y { get; }
                public double Z { get; private set; }
                public virtual void Increment()
                {
                    X++;
                    Z += zIncr;

                    linearInterpolation.IncrementDistance();

                    if (X > x2)
                        HasFinished = true;
                }

                public abstract Color CalculateColor();

                public bool HasFinished { get; private set; } = false;
            }
        }
        //Klasa do interpolacji bez oświetlenia, wypełnienie jednokolorowe
        class VerticalBasicInterpolation : VerticalInterpolation
        {
            public VerticalBasicInterpolation(VertexData from, VertexData to,
                Figure figure, bool perspectiveCorrection) 
                : base(from, to, figure, perspectiveCorrection)
            {
            }

            public override IHorizontalInterpolationData 
                GetHorizontalInterpolationData(IVerticalInterpolationData right)
            {
                return new HorizontalBasicInterpolation(
                    X, right.X, Y, Z, right.Z, CurrentAttributes, 
                    right.CurrentAttributes, figure,
                    perspectiveCorrection);
            }

            protected class HorizontalBasicInterpolation : HorizontalInterpolation
            {
                public HorizontalBasicInterpolation(int left, int right, 
                    int y, double leftZ, double rightZ,
                    PointAttributes leftAttributes, 
                    PointAttributes rightAttributes, Figure figure, 
                    bool perspectiveCorrection) 
                    : base(left, right, y, leftZ, rightZ, 
                        leftAttributes, rightAttributes, figure, perspectiveCorrection)
                {
                }

                public override Color CalculateColor()
                {
                    return figure.Kd;
                }
            }
        }
        //Klasa do interpolacji bez oświetlenia, z nałożoną teksturą
        class VerticalTextureInterpolation : VerticalBasicInterpolation
        {
            public VerticalTextureInterpolation(VertexData from, VertexData to, Figure figure, bool perspectiveCorrection) 
                : base(from, to, figure, perspectiveCorrection)
            {
            }
            public override IHorizontalInterpolationData GetHorizontalInterpolationData(IVerticalInterpolationData right)
            {
                return new HorizontalTextureInterpolation(X, right.X, Y, Z, right.Z, CurrentAttributes,
                    right.CurrentAttributes, figure, perspectiveCorrection);
            }
            public override void Increment()
            {
                base.Increment();

                //Poza rzeczami bazowymi, musimy też zinterpolować atrybuty tekstury
                CurrentAttributes.TextureCoords = linearInterpolation.Interpolate(
                    attributes1.TextureCoords,
                    attributes2.TextureCoords
                );
            }
            protected class HorizontalTextureInterpolation : HorizontalInterpolation
            {
                public HorizontalTextureInterpolation(int left, int right, int y, double leftZ, double rightZ, 
                    PointAttributes leftAttributes, PointAttributes rightAttributes, Figure figure, 
                    bool perspectiveCorrection) : base(left, right, y, leftZ, rightZ, leftAttributes, 
                        rightAttributes, figure, perspectiveCorrection)
                {
                }
                public override void Increment()
                {
                    base.Increment();

                    currentAttributes.TextureCoords = linearInterpolation.Interpolate(
                        attributes1.TextureCoords,
                        attributes2.TextureCoords
                        );
                }

                //Odpowiedni kolor bierzemy z tekstury
                public override Color CalculateColor()
                {
                    var pos = currentAttributes.TextureCoords;
                    int x = Math.Max(Math.Min((int)(figure.Texture.Width * pos.X), figure.Texture.Width - 1),0);
                    int y = Math.Max(Math.Min((int)(figure.Texture.Height * (1-pos.Y)), figure.Texture.Height - 1),0);
                    return figure.Texture.GetPixel(x, y);
                }
            }
        }
        //Klasa do interpolacji z oświetleniem i wypełnieniem jednokolorowym
        class VerticalPhongInterpolation : VerticalInterpolation
        {
            readonly protected  List<Light> lights;
            readonly protected double[] cameraCoords;

            public VerticalPhongInterpolation(
                VertexData from, VertexData to, 
                List<Light> lights, double[] cameraCoords, Figure figure, 
                bool perspectiveCorrection) 
                : base(from, to, figure, perspectiveCorrection)
            {
                this.cameraCoords = cameraCoords;
                this.lights = lights;
            }

            public override void Increment()
            {
                base.Increment();  //Pamiętaj, że tutaj są już ustawione odpowiednie współczynniki w interpolacji liniowej
                CurrentAttributes.Wc = linearInterpolation.Interpolate(
                    attributes1.Wc, attributes2.Wc);
                CurrentAttributes.Normal = linearInterpolation.InterpolateVector(
                    attributes1.Normal, attributes2.Normal);
                CurrentAttributes.WorldCoords = linearInterpolation.Interpolate(
                    attributes1.WorldCoords, attributes2.WorldCoords);
            }

            public override IHorizontalInterpolationData GetHorizontalInterpolationData(IVerticalInterpolationData right)
            {
                var rightData = right as VerticalPhongInterpolation;
                return new HorizontalPhongInterpolation(
                    X, rightData.X, Y, Z, rightData.Z, CurrentAttributes, 
                    right.CurrentAttributes, lights, cameraCoords, figure, 
                    perspectiveCorrection);
            }

            protected class HorizontalPhongInterpolation : HorizontalInterpolation
            {
                readonly protected double[] cameraCoords;
                readonly protected List<Light> lights;

                public HorizontalPhongInterpolation(int left, int right, 
                    int y, double leftZ, double rightZ,
                    PointAttributes leftAttributes, PointAttributes rightAttributes, 
                    List<Light> lights, double[] cameraCoords,
                    Figure figure, bool perspectiveCorrection) : 
                    base(left, right, y, leftZ, rightZ, leftAttributes, 
                        rightAttributes, figure, perspectiveCorrection)
                {
                    this.lights = lights;
                    this.cameraCoords = cameraCoords;
                }

                public override void Increment()
                {
                    base.Increment();  //tutaj zostają już zaktualizowane współczynniki do interpolacji liniowej

                    currentAttributes.Normal = linearInterpolation.InterpolateVector(
                        attributes1.Normal, attributes2.Normal);
                    currentAttributes.WorldCoords = linearInterpolation.Interpolate(
                        attributes1.WorldCoords, attributes2.WorldCoords);
                }

                public override Color CalculateColor()
                {
                    return PhongLightingModel.CalculateColor(figure.NormalizedKa, figure.NormalizedKs,
                        figure.NormalizedKd, figure.Shininess, currentAttributes.WorldCoords, currentAttributes.Normal,
                        lights, cameraCoords);
                }
            }

        }
        //Klasa do interpolacji z oświetleniem i teksturą
        class VerticalTexturePhongInterpolation : VerticalPhongInterpolation
        {
            public VerticalTexturePhongInterpolation(VertexData from, VertexData to, List<Light> lights, 
                double[] cameraCoords, Figure figure, bool perspectiveCorrection) 
                : base(from, to, lights, cameraCoords, figure, perspectiveCorrection)
            {
            }

            public override IHorizontalInterpolationData GetHorizontalInterpolationData(IVerticalInterpolationData right)
            {
                return new HorizontalTexturePhongInterpolation(X, right.X, Y, Z, right.Z, CurrentAttributes,
                    right.CurrentAttributes, lights, cameraCoords, figure, perspectiveCorrection);
            }

            public override void Increment()
            {
                base.Increment();

                CurrentAttributes.TextureCoords = linearInterpolation.Interpolate(attributes1.TextureCoords,
                    attributes2.TextureCoords);
                CurrentAttributes.Binormal = linearInterpolation.InterpolateVector(attributes1.Binormal, attributes2.Binormal);
                CurrentAttributes.Tangent = linearInterpolation.InterpolateVector(attributes1.Tangent, attributes2.Tangent);
            }

            class HorizontalTexturePhongInterpolation : HorizontalPhongInterpolation
            {
                public HorizontalTexturePhongInterpolation(int left, int right, int y, double leftZ, double rightZ, 
                    PointAttributes leftAttributes, PointAttributes rightAttributes, List<Light> lights, 
                    double[] cameraCoords, Figure figure, bool perspectiveCorrection) 
                    : base(left, right, y, leftZ, rightZ, leftAttributes, rightAttributes, 
                          lights, cameraCoords, figure, perspectiveCorrection)
                {
                }

                public override Color CalculateColor()
                {
                    var pos = currentAttributes.TextureCoords;
                    int x = Math.Max(Math.Min((int)(figure.Texture.Width * pos.X), figure.Texture.Width - 1), 0);
                    int y = Math.Max(Math.Min((int)(figure.Texture.Height * (1 - pos.Y)), figure.Texture.Height - 1), 0);
                    var pixel = figure.Texture.GetPixel(x, y);
                    var normalPixel = figure.NormalMap.GetPixel(x, y);
                    double[] N_M = new double[]
                    {
                        (normalPixel.R-127.5d) / 127.5d,
                        (normalPixel.G-127.5d) / 127.5d,
                        (normalPixel.B-127.5d) / 127.5d,
                    };
                    Utils.NormalizeVector(N_M);

                    double[,] TBN = new double[3, 3];
                    for (int i = 0; i < 3; i++)
                    {
                        TBN[i, 0] = currentAttributes.Tangent[i];
                        TBN[i, 1] = currentAttributes.Binormal[i];
                        TBN[i, 2] = currentAttributes.Normal[i];
                    }

                    var N = Utils.MultiplyMatrixVector(TBN, N_M);
                    Utils.NormalizeVector(N);

                    const double tmp = 1d / 255;
                    double[] kd = new double[]
                    {
                        pixel.R*tmp,
                        pixel.G*tmp,
                        pixel.B*tmp
                    };

                    return PhongLightingModel.CalculateColor(figure.NormalizedKa, figure.NormalizedKs,
                        kd, figure.Shininess, currentAttributes.WorldCoords, N,
                        lights, cameraCoords);
                }

                public override void Increment()
                {
                    base.Increment();

                    currentAttributes.TextureCoords = linearInterpolation.Interpolate(attributes1.TextureCoords, 
                        attributes2.TextureCoords);
                    currentAttributes.Binormal = linearInterpolation.InterpolateVector(attributes1.Binormal, attributes2.Binormal);
                    currentAttributes.Tangent = linearInterpolation.InterpolateVector(attributes1.Tangent, attributes2.Tangent);
                }
            }
        }

        //Poniższe klasy sa mi przydatne do interpolacji liniowej. Trzymają one w sobie
        //pomocnicze wartości (np. bez korekcji perspektywy jest to q i 1-q) i umożliwiają
        //aktualizację tych współczynników do interpolacji jedynie za pomocą inkrementacji.
        //W ten sposób, nie muszę za każdym razem wyliczać współczynników do interpolacji od zera.
        interface ILinearIntepolation
        {
            //distance to odległość od pierwszego punktu, od którego interpolujemy
            double[] Interpolate(double[] value1, double[] value2);
            double[] InterpolateVector(double[] vector1, double[] vector2);  //Ta funkcja w porównaniu do powyższej zrobi jeszcze normalizację wyniku
            double Interpolate(double value1, double value2);
            PointF Interpolate(PointF value1, PointF value2);
            void IncrementDistance();
            //Poniższa metoda odpowiada wykonaniu IncrementDistance value razy
            void IncrementDistance(uint value);
        }

        class LinearInterpolation : ILinearIntepolation
        {
            //totalDistance oznacza odległość między punktami, między
            //którymi interpolujemy. Uwaga! ta odległość nie musi być odległością
            //euklidesową, może być np. jedynie różnicą y (albo jedynie różnicą x)
            readonly double totalDistanceInv;
            double coeff1;  //to będzie odpowiadało 1-q ze wzoru
            double coeff2;  //to będzie odpowiadało q ze wzoru
            public LinearInterpolation(int totalDistance)  //totalDistance może być ujemny
            {
                totalDistanceInv = 1d / Math.Abs(totalDistance);
                coeff1 = 1d;
                coeff2 = 0d;
            }

            public void IncrementDistance()
            {
                coeff1 -= totalDistanceInv;
                coeff2 += totalDistanceInv;
            }

            public void IncrementDistance(uint value)
            {
                coeff1 -= value * totalDistanceInv;
                coeff2 += value * totalDistanceInv;
            }

            public double[] Interpolate(double[] value1, double[] value2)  //distance może być jak najbardziej ujemny, ważne żeby distance/totalDistanceInv było od 0 do 1
            {
                var result = Utils.AddVectors(Utils.MultiplyVector(coeff1, value1),
                        Utils.MultiplyVector(coeff2, value2));
                return result;
            }

            public double Interpolate(double value1, double value2)
            {
                return coeff1 * value1 + coeff2 * value2;
            }

            public PointF Interpolate(PointF value1, PointF value2)
            {
                return new PointF((float)Interpolate(value1.X, value2.X), (float)Interpolate(value1.Y, value2.Y));
            }

            public double[] InterpolateVector(double[] vector1, double[] vector2)
            {
                var result = Interpolate(vector1, vector2);
                Utils.NormalizeVector(result);
                return result;
            }
        }

        //Interpolacja liniowa z korekcją perspektywy
        class LinearIntepolationWithPC : ILinearIntepolation
        {
            //W tej interpolacji z korekcją perspektywy będę stosował wzór z pdfa, przy czym
            //licznik i mianownik ze wzoru przmnożyłem przez wc1*wc2*totalDistance
            //coeff1 będzie oznaczać wyrażenie wc2*(totalDistance - distance)
            //zaś coeff2 będzie oznaczać aktualny wc1*distance, gdzie q = distance/totalDistance
            readonly double wc1;
            readonly double wc2;
            double coeff1;
            double coeff2;

            public LinearIntepolationWithPC(double wc1, double wc2, int totalDistance)
            {
                this.wc1 = wc1;
                this.wc2 = wc2;
                coeff1 = wc2 * Math.Abs(totalDistance);
                coeff2 = 0d;
            }

            public void IncrementDistance()
            {
                coeff1 -= wc2;
                coeff2 += wc1;
            }

            public void IncrementDistance(uint value)
            {
                coeff1 -= value * wc2;
                coeff2 += value * wc1;
            }

            public double[] Interpolate(double[] value1, double[] value2)
            {
                var result =  
                    Utils.MultiplyVector(
                        1d / (coeff1 + coeff2),
                        Utils.AddVectors(
                            Utils.MultiplyVector(coeff1, value1),
                            Utils.MultiplyVector(coeff2, value2)
                        )
                    );
                return result;
            }

            public double Interpolate(double value1, double value2)
            {
                return (coeff1 * value1 + coeff2 * value2) / (coeff1 + coeff2);
            }

            public PointF Interpolate(PointF value1, PointF value2)
            {
                return new PointF((float)Interpolate(value1.X, value2.X), (float)Interpolate(value1.Y, value2.Y));
            }

            public double[] InterpolateVector(double[] vector1, double[] vector2)
            {
                var result = Interpolate(vector1, vector2);
                Utils.NormalizeVector(result);
                return result;
            }
        }
    }
}
