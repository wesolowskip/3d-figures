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
    class Cylinder : Figure
    {
        private int _r;
        private int _h;
        private int _circlePointsCount;

        [Category("Parametry figury")]
        [Description("Promień podstawy")]
        public int R { get => _r; set { _r = value; UpdateTrianglesAndWorldsData(); } }
        [Description("Wysokość")]
        [Category("Parametry figury")]
        public int H { get => _h; set { _h = value; UpdateTrianglesAndWorldsData(); } }
        [Description("Liczba podziałów okręgu podstawy")]
        [Category("Parametry figury")]
        public int CirclePointsCount { get => _circlePointsCount; set { _circlePointsCount = value; UpdateTrianglesAndWorldsData(); } }

        public override string Name { get; } = "Walec";

        public Cylinder(int r, int h, int circlePointsCount)
        {
            _r = r;
            _h = h;
            _circlePointsCount = circlePointsCount;
            UpdateTrianglesAndWorldsData();
        }

        protected override void UpdateTriangles()
        {
            int pointsCount = (CirclePointsCount << 1) + 2;
            int trianglesCount = CirclePointsCount << 2; //obie podstawy + powierzchnia boczna
            InitializeArrays(trianglesCount);
            modelPointsCoords = new double[4,pointsCount];

            double phiIncr = 2 * Math.PI / CirclePointsCount;
            double phi = 0d;
            var tangents = new List<double[]>();   //pomocnicze listy będą zawierać wektory dla punktów tylko w kontekście powierzchni bocznej (czyli styczne, binormale, normalne ale tylko na powierzchni bocznej, bez podstaw)
            var binormals = new List<double[]>();
            var normals = new List<double[]>();
            var upperBaseBinormals = new List<double[]>();
            var lowerBaseBinormals = new List<double[]>();
            var upperBaseNormal = new double[] { 0d, 1d, 0d };
            var lowerBaseNormal = new double[] { 0d, -1d, 0d };

            var texturePoints = new List<PointF>();
            float textureLowerBaseY = (float)R / (2 * R + H);
            float textureUpperBaseY = 1f - textureLowerBaseY;
            float textureX = 0f;
            float textureXIncr = 1f / CirclePointsCount;

            for (int i = 0; i < CirclePointsCount; i++)
            {
                var sinPhi = Math.Sin(phi);
                var cosPhi = Math.Cos(phi);
                var point = new double[]
                {
                    R*cosPhi,
                    H/2,
                    R*sinPhi,
                    1
                };
                PutVertexIntoModelPointsCoords(point, i<<1);
                var tangent = new double[]  //znormalizowany od razu
                {
                    -sinPhi,
                    0d,
                    cosPhi
                };
                var binormal = new double[]
                {
                    0d,
                    1d,
                    0d
                };
                var normal = new double[]   //znormalizowany od razu
                {
                    cosPhi,
                    0d,
                    sinPhi
                };
                tangents.Add(tangent);
                binormals.Add(binormal);
                normals.Add(normal);
                texturePoints.Add(new PointF(textureX, textureUpperBaseY));
                point[1] = -point[1];
                PutVertexIntoModelPointsCoords(point, (i << 1) + 1); 
                tangents.Add(tangent);  //punkt na brzegu dolnej podstawy ma takie same wektory jak odpowiadajacy punkt na brzegu górnej podstawy (mówimy oczywiście o wektorach w kontekście powierzchni bocznej)
                binormals.Add(binormal);
                normals.Add(normal);
                texturePoints.Add(new PointF(textureX, textureLowerBaseY));

                upperBaseBinormals.Add(Utils.CrossProduct(tangent, upperBaseNormal));
                lowerBaseBinormals.Add(Utils.CrossProduct(tangent, lowerBaseNormal));

                phi += phiIncr;
                textureX += textureXIncr;
            }
            PutVertexIntoModelPointsCoords(new double[] { 0, H/2, 0, 1 }, pointsCount - 2);  //środek górnej podstawy
            PutVertexIntoModelPointsCoords(new double[] { 0, -H/2, 0, 1 }, pointsCount - 1);  //środek dolnej podstawy
            var upperBaseTangent = new double[]
            {
                1, 0, 0
            };
            var upperBaseBinormal = new double[]
            {
                0,0,1
            };
            int index = 0;
            for (int i=0; i<CirclePointsCount; i++)  //trójkąty z górnej podstawy
            {
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    i << 1,  tangents[i], upperBaseBinormals[i],
                    upperBaseNormal, texturePoints[i<<1], index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    ((i << 1) + 2) % (CirclePointsCount<<1), tangents[(i+1)%CirclePointsCount], 
                    upperBaseBinormals[(i + 1) % CirclePointsCount],
                    upperBaseNormal, texturePoints[((i << 1) + 2) % (CirclePointsCount << 1)], 
                    index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointsCount - 2, upperBaseTangent, upperBaseBinormal,
                    upperBaseNormal, new PointF(texturePoints[i << 1].X, 1f), index);  //środek górnej podstawy
                index++;
            }
            TextureCoords[index - 2].X = 1f;  //To jest bardzo ważne! (poprawiamy ostatni wierzchołek w ostatnim trójkącie żeby potem się dobrze interpolowało, czyli ten ostatni wierzchołek w jednym trójkącie ma współrzędną tekstury X 0f, a w drugim 1f)
            var lowerBaseTangent = new double[]
            {
                1, 0, 0
            };
            var lowerBaseBinormal = new double[]
            {
                0,0,-1
            };
            //teraz trójkąty z dolnej podstawy
            for (int i = 0; i < CirclePointsCount; i++) 
            {
                //inna kolejność niż w pętli powyżej, by zachować clockwise
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    ((i << 1) + 3) % (CirclePointsCount << 1), tangents[(i+1)%CirclePointsCount], 
                    lowerBaseBinormals[(i + 1) % CirclePointsCount], lowerBaseNormal, 
                    texturePoints[((i << 1) + 3) % (CirclePointsCount << 1)], index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    (i << 1) + 1, tangents[i],
                    lowerBaseBinormals[i], lowerBaseNormal, 
                    texturePoints[(i << 1) + 1], index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointsCount - 1, lowerBaseTangent, lowerBaseBinormal, 
                    lowerBaseNormal, new PointF(texturePoints[(i << 1) + 1].X, 0f), index);  //środek dolnej podstawy
                index++;
            }
            TextureCoords[index - 3].X = 1f;
            //Pozostało nam potworzyć trójkąty na ścianie bocznej, 
            //każdy prostokąt pionowy będzie się składał z dwóch trójkątów
            for (int i=0; i<CirclePointsCount; i++)
            {
                //najpierw dodajemy pierwszy trójkąt tworzący pionowy prostokąt na powierzchni bocznej
                int pointIdx = i << 1;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointIdx, tangents[pointIdx], binormals[pointIdx], 
                    normals[pointIdx], texturePoints[pointIdx], index);
                index++;    //górny prawy wierzchołek pionowego prostokąta łączącego podstawy
                pointIdx++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointIdx, tangents[pointIdx], binormals[pointIdx], 
                    normals[pointIdx], texturePoints[pointIdx], index);  //dolny prawy wierzchołek prostokąta
                index++;
                pointIdx = (pointIdx + 1) % (CirclePointsCount << 1);
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointIdx, tangents[pointIdx], binormals[pointIdx],
                    normals[pointIdx], texturePoints[pointIdx], index);  //górny lewy wierzchołek prostokąta
                index++;

                //teraz drugi trójkąt z prostokąta
                pointIdx = (i << 1) + 1;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointIdx, tangents[pointIdx], binormals[pointIdx],
                    normals[pointIdx], texturePoints[pointIdx], index);  //dolny prawy wierzchołek prostokąta na boku
                index++;
                pointIdx = (pointIdx + 2) % (CirclePointsCount << 1);
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointIdx, tangents[pointIdx], binormals[pointIdx],
                    normals[pointIdx], texturePoints[pointIdx], index);  //dolny lewy wierzchołek prostokąta
                index++;
                pointIdx = pointIdx - 1;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointIdx, tangents[pointIdx], binormals[pointIdx],
                    normals[pointIdx], texturePoints[pointIdx], index);  //górny lewy wierzchołek prostokąta
                index++;
            }
            TextureCoords[index - 1].X = 1f; ;
            TextureCoords[index - 2].X = 1f;
            TextureCoords[index - 4].X = 1f;
        }
    }
}
