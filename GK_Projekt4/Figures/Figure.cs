using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4.Figures
{
    [Serializable]
    abstract class Figure : INameable
    {
        public enum FillType
        {
            None,
            SingleColor,
            Texture
        }

        private double _sx = 1;
        private double _sy = 1;
        private double _sz = 1;
        private double _tx;   //zainicjowane zerami
        private double _ty;
        private double _tz;
        private int rx;
        private int ry;
        private int rz;

        //każde 3 kolejne elementy tablicy Triangles (tj. 0,1 i 2, potem 3,4 i 5 itd.) 
        //to wierzchołki kolejnych trójkątów zorientowanych clockwise. 
        //Są to konkretnie indeksy elementów tablicy PointsCoords (niezależnie 
        //czy modelPointsCoords czy worldPointsCorrds), czyli 
        //np. Triangles[0]=2 oznacza, że pierwszy wierzchołek pierwszego 
        //trójkąta na współrzędne w kolumnie o indeksie 2 w PointsCoords
        [Browsable(false)]
        public int[] Triangles { get; protected set; }
        //i-ty wierzchołek z tablicy Triangles ma swoje odpowiadające
        //i-te wektory z poniższych tablic
        protected double[,] modelNormals;
        protected double[,] modelBinormals;
        protected double[,] modelTangents;
        protected double[,] modelPointsCoords;
        private double[,] modelMatrix;
        private double[,] normalMatrix;
        private Color _ks = Color.White;
        private Color _kd = Color.Blue;
        private Color _ka = Color.Gainsboro;
        private FillType _fill = FillType.SingleColor;
        [NonSerialized]
        private LockBitmap _texture;
        [NonSerialized]
        private LockBitmap _normalMap;
        private string _texturePath;
        private string _normalMapPath;

        protected Figure()
        {
            UpdateModelMatrix();
            UpdateNormalizedKa();
            UpdateNormalizedKd();
            UpdateNormalizedKs();
        }

        [Browsable(false)]
        public PointF[] TextureCoords { get; protected set; }
        [Browsable(false)]
        public double[,] WorldNormals { get; protected set; }
        [Browsable(false)]
        public double[,] WorldBinormals { get; protected set; }
        [Browsable(false)]
        public double[,] WorldTangents { get; protected set; }
        [Browsable(false)]
        public double[,] WorldPointsCoords { get; protected set; }
        [Category("Transformacje")]
        [Description("Współczynnik skali względem OX")]
        public double Sx
        {
            get => _sx; set
            {
                _sx = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Współczynnik skali względem OY")]
        public double Sy
        {
            get => _sy; set
            {
                _sy = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Współczynnik skali względem OZ")]
        public double Sz
        {
            get => _sz; set
            {
                _sz = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Kąt obrotu wokół OX w stopniach")]
        public int Rx
        {
            get => rx; set
            {
                rx = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Kąt obrotu wokół OY w stopniach")]
        public int Ry
        {
            get => ry; set
            {
                ry = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Kąt obrotu wokół OZ w stopniach")]
        public int Rz
        {
            get => rz; set
            {
                rz = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Translacja wzdłuż OX")]
        public double Tx
        {
            get => _tx; set
            {
                _tx = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Translacja wzdłuż OY")]
        public double Ty
        {
            get => _ty; set
            {
                _ty = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Category("Transformacje")]
        [Description("Translacja wzdłuż OZ")]
        public double Tz
        {
            get => _tz; set
            {
                _tz = value;
                UpdateModelMatrixAndWorldsData();
            }
        }
        [Description("Nazwa figury")]
        abstract public string Name { get; }
        [Category("Wypełnienie")]
        [Description("Sposób wypełnienia powierzchni figury")]
        public FillType Fill
        {
            get => _fill;
            set
            {
                if (value == FillType.Texture && (Texture == null || NormalMap == null))
                    throw new Exception("Najpierw należy ustawić teksturę oraz mapę wysokości");
                _fill = value;
            }
        }
        [Category("Światło")]
        [Description("Współczynnik składowej odbić lustrzanych")]
        public Color Ks { get => _ks; set { _ks = value; UpdateNormalizedKs(); } }
        [Category("Światło")]
        [Description("Współczynnik składowej rozproszonej")]
        public Color Kd { get => _kd; set { _kd = value; UpdateNormalizedKd(); } }
        [Category("Światło")]
        [Description("Współczynnik składowej oświetlenia otoczenia")]
        public Color Ka { get => _ka; set { _ka = value; UpdateNormalizedKa(); } }
        [Category("Światło")]
        [Description("Współczynnik połysku")]
        public uint Shininess { get; set; } = 50;

        public override string ToString() => Name;

        [Category("Wypełnienie")]
        [Description("Plik z teksturą")]
        [Editor(typeof(CustomFileBrowser), typeof(System.Drawing.Design.UITypeEditor))]
        public string TexturePath
        {
            get => _texturePath;
            set
            {
                //Poniższy kod jest do zmiany ścieżki bezwzględnej na względną
                var projectPath = Path.GetFullPath(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    @"..\.."));
                int index = value.IndexOf(projectPath);
                if (value.Length > 0 && index == 0)
                {
                    value = @"..\.." + value.Substring(projectPath.Length);
                }
                if (_texturePath != value)
                {
                    _texturePath = value;
                    Texture = new LockBitmap(new Bitmap(_texturePath));
                }
            }
        }

        [Category("Wypełnienie")]
        [Description("Plik z mapą normalną")]
        [Editor(typeof(CustomFileBrowser), typeof(System.Drawing.Design.UITypeEditor))]
        public string NormalMapPath
        {
            get => _normalMapPath;
            set
            {
                //Poniższy kod jest do zmiany ścieżki bezwzględnej na względną
                var projectPath = Path.GetFullPath(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    @"..\.."));
                int index = value.IndexOf(projectPath);
                if (value.Length > 0 && index == 0)
                {
                    value = @"..\.." + value.Substring(projectPath.Length);
                }
                if (_normalMapPath != value)
                {
                    _normalMapPath = value;
                    NormalMap = new LockBitmap(new Bitmap(_normalMapPath));
                }
            }
        }

        [Browsable(false)]
        public LockBitmap Texture
        {
            get => _texture;
            private set
            {
                if (_texture != value)
                {
                    _texture?.Dispose();
                    _texture = value;
                }
            }
        }

        [Browsable(false)]
        public LockBitmap NormalMap
        {
            get => _normalMap;
            private set
            {
                if (_normalMap != value)
                {
                    _normalMap?.Dispose();
                    _normalMap = value;
                }
            }
        }

        [Browsable(false)]
        public double[] NormalizedKa { get; set; } = new double[3];
        [Browsable(false)]
        public double[] NormalizedKd { get; set; } = new double[3];
        [Browsable(false)]
        public double[] NormalizedKs { get; set; } = new double[3];
        private void UpdateModelMatrix()
        {
            var s = new double[,]
            {
                {Sx,0,0,0 },
                {0,Sy,0,0 },
                {0,0,Sz,0 },
                {0,0,0,1 }
            };
            double cosAngleX = Math.Cos(Rx * Math.PI / 180d);
            double sinAngleX = Math.Sin(Rx * Math.PI / 180d);
            var r_x = new double[,]
            {
                {1,0,0,0 },
                { 0, cosAngleX, -sinAngleX, 0},
                {0, sinAngleX, cosAngleX, 0 },
                {0,0,0,1 }
            };
            double cosAngleY = Math.Cos(Ry * Math.PI / 180d);
            double sinAngleY = Math.Sin(Ry * Math.PI / 180d);
            var r_y = new double[,]
            {
                {cosAngleY,0,-sinAngleY,0 },
                { 0, 1, 0, 0},
                {sinAngleY, 0, cosAngleY, 0 },
                {0,0,0,1 }
            };
            double cosAngleZ = Math.Cos(Rz * Math.PI / 180d);
            double sinAngleZ = Math.Sin(Rz * Math.PI / 180d);
            var r_z = new double[,]
            {
                {cosAngleZ,-sinAngleZ,0,0 },
                { sinAngleZ, cosAngleZ, 0, 0},
                {0, 0, 1, 0 },
                {0,0,0,1 }
            };
            var t = new double[,]
            {
                {1,0,0,Tx },
                {0,1,0,Ty },
                {0,0,1,Tz },
                {0,0,0,1 }
            };
            modelMatrix = Utils.MultiplyMatrices(t, r_x, r_y, r_z, s);
            normalMatrix = Utils.TransposeMatrix(
                Utils.Inverse3x3Matrix(modelMatrix));   //funkcja Inverse3x3Matrix jest napisana tak, że dla większej macierzy odwróci tylko jej lewą górną podmacierz 3x3
        }
        protected void InitializeArrays(int trianglesCount)
        {
            int triangleVerticesCount = 3 * trianglesCount;
            Triangles = new int[triangleVerticesCount];
            TextureCoords = new PointF[triangleVerticesCount];
            modelNormals = new double[3, triangleVerticesCount];
            modelBinormals = new double[3, triangleVerticesCount];
            modelTangents = new double[3, triangleVerticesCount];
        }
        protected void PutVertexIntoModelPointsCoords(double[] vertex, int index)
        {
            for (int i = 0; i < 4; i++)
                modelPointsCoords[i, index] = vertex[i];
        }
        protected void PutVertexCoordsIndexIntoTriangles(int coordsIdx, int index)
        {
            Triangles[index] = coordsIdx;
        }
        private void PutVectorIntoArray(double[] vector, int index, double[,] array)
        {
            //Wpisujemy tylko 3 pierwsze wiersze w tablicy,
            //ostatni wiersz jest wypełniony zerami
            for (int i = 0; i < 3; i++)
                array[i, index] = vector[i];
        }
        protected void PutVertexAndCorrespondingAttributesIntoTriangleArrays(int vertexCoordsIndex,
            double[] tangent, double[] binormal, double[] normal, PointF texturePoint, int index)
        {
            PutVertexCoordsIndexIntoTriangles(vertexCoordsIndex, index);
            TextureCoords[index] = texturePoint;
            PutVectorIntoArray(normal, index, modelNormals);
            PutVectorIntoArray(tangent, index, modelTangents);
            PutVectorIntoArray(binormal, index, modelBinormals);
        }
        protected void UpdateWorldData()
        {
            WorldPointsCoords = Utils.MultiplyMatrices(modelMatrix,
                modelPointsCoords);
            WorldNormals = Utils.MultiplyMatrices(normalMatrix,
                modelNormals);   
            WorldBinormals = Utils.MultiplyMatrices(normalMatrix,
                modelBinormals);
            WorldTangents = Utils.MultiplyMatrices(normalMatrix,
                modelTangents);
            Utils.NormalizeVectors(WorldNormals);
            Utils.NormalizeVectors(WorldBinormals);
            Utils.NormalizeVectors(WorldTangents);
        }
        protected abstract void UpdateTriangles();
        protected void UpdateTrianglesAndWorldsData()
        {
            UpdateTriangles();
            UpdateWorldData();
        }
        protected void UpdateModelMatrixAndWorldsData()
        {
            UpdateModelMatrix();
            UpdateWorldData();
        }
        private void UpdateNormalizedKa()
        {
            const double tmp = 1d / 255;
            NormalizedKa[0] = Ka.R * tmp;
            NormalizedKa[1] = Ka.G * tmp;
            NormalizedKa[2] = Ka.B * tmp;
        }
        private void UpdateNormalizedKd()
        {
            const double tmp = 1d / 255;
            NormalizedKd[0] = Kd.R * tmp;
            NormalizedKd[1] = Kd.G * tmp;
            NormalizedKd[2] = Kd.B * tmp;
        }
        private void UpdateNormalizedKs()
        {
            const double tmp = 1d / 255;
            NormalizedKs[0] = Ks.R * tmp;
            NormalizedKs[1] = Ks.G * tmp;
            NormalizedKs[2] = Ks.B * tmp;
        }

        [OnDeserialized]
        private void FigureOnDeserialized(StreamingContext context)
        {
            try
            {
                Texture = new LockBitmap(new Bitmap(_texturePath));
                NormalMap = new LockBitmap(new Bitmap(_normalMapPath));
            }
            catch(Exception)
            {
                //Jeśli coś się nie powiodło z teksturami, to zmieniamy sposób wypełnienia na jednokolorowy
                //oraz resetujemy ścieżki tekstur i same tekstury
                if (Fill == FillType.Texture)
                    Fill = FillType.SingleColor;
                Texture?.Dispose();
                NormalMap?.Dispose();
                Texture = NormalMap = null;
                _texturePath = _normalMapPath = string.Empty;
            }
        }
    }
}
