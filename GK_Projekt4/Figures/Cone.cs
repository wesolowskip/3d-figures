using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4.Figures
{
    [Serializable]
    class Cone:Figure
    {
        private int _r;
        private int _h;
        private int _circlePointsCount;

        [Category("Parametry figury")]
        [Description("Promień podstawy")]
        public int R { get => _r; set { _r = value; UpdateTrianglesAndWorldsData(); } }
        [Category("Parametry figury")]
        [Description("Wysokość")]
        public int H { get => _h; set { _h = value; UpdateTrianglesAndWorldsData(); } }
        [Category("Parametry figury")]
        [Description("Liczba podziałów okręgu podstawy")]
        public int CirclePointsCount { get => _circlePointsCount; set { _circlePointsCount = value; UpdateTrianglesAndWorldsData(); } }

        public override string Name { get;  } = "Stożek";

        public Cone(int r, int h, int circlePointsCount)
        {
            _r = r;
            _h = h;
            _circlePointsCount = circlePointsCount;
            UpdateTrianglesAndWorldsData();
        }

        protected override void UpdateTriangles()
        {
            int pointsCount = CirclePointsCount + 2;
            int trianglesCount = CirclePointsCount << 1; //obie podstawy + powierzchnia boczna
            InitializeArrays(trianglesCount);
            modelPointsCoords = new double[4, pointsCount];
            var tangents = new List<double[]>();
            var binormals = new List<double[]>();
            var normals = new List<double[]>();
            var texturePoints = new List<PointF>();
            var tangentsInConeVertex = new List<double[]>();
            var normalsInConeVertex = new List<double[]>();
            var binormalsInConeVertex = new List<double[]>();
            double phiIncr = 2 * Math.PI / CirclePointsCount;
            double phi = 0d;

            float textureX = 0f;
            float baseEdgeTextureY = R / (float)Math.Sqrt(R*R+H*H);
            float textureIncr = 1f / CirclePointsCount;

            double[] baseNormal = new double[] { 0, -1, 0 };
            var baseBinormals = new List<double[]>();

            for (int i=0; i<CirclePointsCount; i++)
            {
                var sinPhi = Math.Sin(phi);
                var cosPhi = Math.Cos(phi);
                var point = new double[]
                {
                    R*cosPhi,
                    0d,
                    R*sinPhi,
                    1d
                };
                PutVertexIntoModelPointsCoords(point, i);
                var tangent = new double[]  //znormalizowany od razu
                {
                    -sinPhi,
                    0d,
                    cosPhi
                };
                tangents.Add(tangent);
                baseBinormals.Add(Utils.CrossProduct(tangent, baseNormal));
                var binormal = new double[]
                {
                    -R*cosPhi,
                    H,
                    -R*sinPhi
                };
                Utils.NormalizeVector(binormal);
                binormals.Add(binormal);
                var normal = Utils.CrossProduct(binormal, tangent);
                normals.Add(normal);  //normal jest juz znormalizowany

                texturePoints.Add(new PointF(textureX, baseEdgeTextureY));   //UWAGA texturePoints dodaję tylko dla punktów z podstawy

                //Teraz jeszcze policzymy wektory dla wierzchołka jednego trójkąta z tych,
                //które stanowią powierzchnię boczną stożka. Liczymy zatem
                //wektory dla wierzchołka stożka i konkretnego trójkąta.
                double psi = phi + phiIncr / 2d;
                var sinPsi = Math.Sin(psi);
                var cosPsi = Math.Cos(psi);
                tangent = new double[]  //znormalizowany od razu
                {
                    -sinPsi,
                    0d,
                    cosPsi
                };
                binormal = new double[]
                {
                    -R*cosPsi,
                    H,
                    -R*sinPsi
                };
                Utils.NormalizeVector(binormal);
                normal = Utils.CrossProduct(binormal, tangent);
                tangentsInConeVertex.Add(tangent);
                binormalsInConeVertex.Add(binormal);
                normalsInConeVertex.Add(normal);

                phi += phiIncr;
                textureX += textureIncr;
            }
            //Wierzchołek stożka odpowiada lewemu górnemu rogowi tekstury
            //Środek podstawy stożka ma współrzędne tekstury odpowiadające prawemu dolnemu rogowi
            //Zaś punkty z okręgu podstawy będą odpowiadać współrzędnym na przekątnej tekstury poprowadzonej
            //z lewego dolnego rogu do górnego prawego
            PutVertexIntoModelPointsCoords(new double[] { 0, H, 0, 1 }, pointsCount - 2);
            PutVertexIntoModelPointsCoords(new double[] { 0, 0, 0, 1 }, pointsCount - 1);
            var baseTangent = new double[]
            {
                0, 0, 1
            };
            var baseBinormal = new double[]
            {
                1,0,0
            };
            int index = 0;
            //Najpierw tworzymy trójkąty tworzące podstawę stożka
            for (int i=0; i<CirclePointsCount; i++)
            {
                //umieszczam pojedynczy trójkąt z podstawy, dbam o kolejność clockwise
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    i, tangents[i], baseBinormals[i], baseNormal, texturePoints[i], index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(  //to jest środek podstawy
                    pointsCount - 1, baseTangent, baseBinormal, baseNormal, new PointF(texturePoints[i].X, 0f), index);
                index++;   
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    (i + 1) % CirclePointsCount, tangents[(i + 1) % CirclePointsCount], 
                    baseBinormals[(i + 1) % CirclePointsCount], 
                    baseNormal, texturePoints[(i + 1) % CirclePointsCount], index);
                index++;
            }
            TextureCoords[index - 1].X = 1f;  //Jest to konieczne! Ostatni punkt w ostatnim trójkącie musi mieć tutaj współrzędną (1f, 1f), a nie (0f, 0f), gdyż zależy mi na poprawnej interpolacji później dla tego ostatniego trójkąta
            //Teraz tworzymy trójkąty tworzące powierzchnię boczną stożka
            for (int i=0; i<CirclePointsCount; i++)
            {
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(  //to jest wierzchołek stożka
                    pointsCount - 2, tangentsInConeVertex[i],
                    binormalsInConeVertex[i], normalsInConeVertex[i], 
                    new PointF(texturePoints[i].X, 1f), index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    i, tangents[i], binormals[i], normals[i], texturePoints[i],
                    index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    (i + 1) % CirclePointsCount, 
                    tangents[(i + 1) % CirclePointsCount], 
                    binormals[(i + 1) % CirclePointsCount],
                    normals[(i + 1) % CirclePointsCount], 
                    texturePoints[(i + 1) % CirclePointsCount],
                    index);
                index++;
            }
            TextureCoords[index - 1].X = 1f;
        }
    }
}
