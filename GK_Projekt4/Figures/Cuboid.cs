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
    class Cuboid : Figure
    {
        private int _a;
        private int _b;
        private int _c;

        [Category("Parametry figury")]
        [Description("Długość boku równoległego do OX")]
        public int A { get => _a; set { _a = value; UpdateTrianglesAndWorldsData(); } }  //dlugosc X
        [Category("Parametry figury")]
        [Description("Długość boku równoległego do OY")]
        public int B { get => _b; set { _b = value; UpdateTrianglesAndWorldsData(); } }  //dlugosc Y
        [Category("Parametry figury")]
        [Description("Długość boku równoległego do OZ")]
        public int C { get => _c; set { _c = value; UpdateTrianglesAndWorldsData(); } }  //dlugosc Z

        public override string Name { get; }= "Prostopadłościan";

        public Cuboid(int a, int b, int c)
        {
            _a = a;
            _b = b;
            _c = c;
            InitializeArrays(12);   //na każdej ścianie po 2 trójkąty
            modelPointsCoords = new double[4, 8];
            var normalsForFaces = new double[,]{
                  {0,1, 0,-1,0, 0 },
                  {0,0, 0, 0,1,-1 },
                  {1,0,-1, 0,0, 0 }
              };
            var tangentsForFaces = new double[,]
            {
                  {1, 0,-1,0,1,0 },
                  {0, 0, 0,0,0, 0 },
                  {0,-1, 0,1,0, 1 }
            };
            var binormalsForFaces = new double[,]
            {
                  {0,0,0,0,0,-1 },
                  {1,1,1,1,0,0 },
                  {0,0,0,0,-1,0 }
            };
            var texturePoints = new PointF[]
            {
                new PointF(0f,0f), new PointF(0f,1f), new PointF(1f,1f), new PointF(1f,0f)
            };
            for (int i = 0; i < normalsForFaces.GetLength(0); i++)
                for (int j = 0; j < normalsForFaces.GetLength(1); j++)
                {
                    for (int k = j * 6, l=0; l < 6; k++, l++)  //każda ściana odpowiada 6 punktom
                    {
                        modelNormals[i, k] = normalsForFaces[i, j];
                        modelTangents[i, k] = tangentsForFaces[i, j];
                        modelBinormals[i, k] = binormalsForFaces[i, j];
                        //Jeszcze dodaję współrzędne tekstury, pierwsze 3 punkty reprezentują pierwsze 3 punkty na ścianie,
                        //kolejne 3 reprezenetuję pozostałe punkty na ścianie
                        if (l < 3)
                            TextureCoords[k] = texturePoints[l];
                        else
                            TextureCoords[k] = texturePoints[(l - 1) & 3];   //&3 to to samo co %4
                    }
                }
            UpdateTrianglesAndWorldsData();
        }
        protected override void UpdateTriangles()
        {
            var facesVerticesIndices = new int[][]
            {
                  new []{3,7,6,2 },
                  new []{2,6,5,1 },
                  new []{1,5,4,0 },
                  new []{0,4,7,3 },
                  new []{7,4,5,6 },
                  new []{1,0,3,2 }
            };
            var vertices = new double[][]
            {
                  new double[]{ -A/2, -B/2, -C/2, 1 },
                  new double[]{ A/2, -B/2, -C/2, 1 },
                  new double[]{ A/2, -B/2, C/2, 1 },
                  new double[]{ -A/2, -B/2, C/2, 1 },
                  new double[]{ -A/2, B/2, -C/2, 1 },
                  new double[]{ A/2, B/2, -C/2, 1 },
                  new double[]{ A/2, B/2, C/2, 1 },
                  new double[]{ -A/2, B/2, C/2, 1 },
            };
            for (int i = 0; i < vertices.Length; i++)
                PutVertexIntoModelPointsCoords(vertices[i], i);
            for (int i = 0; i < 6; i++)
                AddTrianglesFromFace(facesVerticesIndices[i], i);
        }
        //v1, v2, v3, v4 sa zgodnie ze wskazowkami zegara patrzac na te sciane
        //  z zewnatrz
          private void AddTrianglesFromFace(int[] faceVerticesIndices, int index)
        {
            index = index * 6;
            for (int i = 0; i != 3; i++)
                PutVertexCoordsIndexIntoTriangles(faceVerticesIndices[i], index++);
            for (int i = 2; i != 1; i = (i + 1) & 3)  //&3 to modulo 4
                PutVertexCoordsIndexIntoTriangles(faceVerticesIndices[i], index++);
        }

    }
}
