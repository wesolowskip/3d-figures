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
    class Sphere : Figure
    {
        private int _r;
        private int _parallelsCount;
        private int _meridiansCount;

        [Category("Parametry figury")]
        [Description("Promień")]
        public int R { get => _r; set { _r = value; UpdateTrianglesAndWorldsData(); } }
        [Category("Parametry figury")]
        [Description("Liczba równoleżników")]
        public int ParallelsCount { get => _parallelsCount; set { _parallelsCount = value; UpdateTrianglesAndWorldsData(); } } //liczba równoleżników
        [Category("Parametry figury")]
        [Description("Liczba południków")]
        public int MeridiansCount { get => _meridiansCount; set { _meridiansCount = value; UpdateTrianglesAndWorldsData(); } }  //liczba południków

        public override string Name { get; } = "Sfera";

        public Sphere(int r, int parallelsCount, int meridiansCount)
        {
            _r = r;
            _parallelsCount = parallelsCount;
            _meridiansCount = meridiansCount;
            UpdateTrianglesAndWorldsData();
        }

        protected override void UpdateTriangles()
        {
            int trianglesCount = CountTriangles();
            int pointsCount = 2 + MeridiansCount * (ParallelsCount - 2);
            InitializeArrays(trianglesCount);
            modelPointsCoords = new double[4, pointsCount];

            double phiIncr = 2 * Math.PI / MeridiansCount,
                psiIncr = Math.PI / (ParallelsCount - 1);
            List<double[]> tangents = new List<double[]>();
            List<double[]> normals = new List<double[]>();
            List<double[]> binormals = new List<double[]>();
            var texturePoints = new List<PointF>();

            PutVertexIntoModelPointsCoords(new double[] { 0, R, 0, 1 }, 0);
            tangents.Add(new double[] { 1, 0, 0 });
            binormals.Add(new double[] { 0, 0, 1 });
            normals.Add(new double[] { 0, 1, 0 });
            texturePoints.Add(new PointF(0f, 1f));  //nie będę tego używał, ale dodaję dla jednolitości indeksów
            //wektor normalny to będzie binormalny razy styczny wektorowo
            double phi, psi = psiIncr;
            int index = 1;

            float textureXIncr = 1f / MeridiansCount;
            float textureYIncr = -1f / (ParallelsCount - 1);
            float textureY = 1f + textureYIncr;
            float textureX;


            for (int i=0; i<ParallelsCount - 2; i++)
            {
                phi = 0d;
                textureX = 0f;
                for (int j=0; j<MeridiansCount; j++)
                {
                    double sinPsi = Math.Sin(psi);
                    double cosPsi = Math.Cos(psi);
                    double sinPhi = Math.Sin(phi);
                    double cosPhi = Math.Cos(phi);
                    PutVertexIntoModelPointsCoords(new double[] 
                    {
                        R * sinPsi * cosPhi,
                        R * cosPsi,
                        R * sinPsi * sinPhi,
                        1
                    }, index);
                    index++;
                    var tangent = new double[]
                    {
                        R*cosPsi*cosPhi,
                        -R*sinPsi,
                        R*cosPsi*sinPhi
                    };
                    Utils.NormalizeVector(tangent);
                    tangents.Add(tangent);
                    var binormal = new double[]
                    {
                        -R*sinPsi*sinPhi,
                        0d,
                        R*sinPsi*cosPhi
                    };
                    Utils.NormalizeVector(binormal);
                    binormals.Add(binormal);
                    phi += phiIncr;
                    var normal = Utils.CrossProduct(binormal, tangent);
                    Utils.NormalizeVector(normal);
                    normals.Add(normal);

                    texturePoints.Add(new PointF(textureX, textureY));

                    textureX += textureXIncr;
                }
                psi += psiIncr;
                textureY += textureYIncr;
            }
            PutVertexIntoModelPointsCoords(new double[] { 0, -R, 0, 1 }, index);
            tangents.Add(new double[] { -1, 0, 0 });
            binormals.Add(new double[] { 0, 0, 1 });
            normals.Add(new double[] { 0, -1, 0 });
            texturePoints.Add(new PointF(0f, 0f)); //nie będę tego używał, ale dodaję dla jednolitości
            index = 0;
            for (int i=1, ii=2; i<=MeridiansCount; i++, ii++)
            {
                //tworzymy trójkąt z biegunu północnego oraz dwóch na równoleżniku poniżej
                //dbam o kolejność clockwise
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(0, tangents[0],
                    binormals[0], normals[0], new PointF(texturePoints[i].X, 1f), index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(i, tangents[i],
                    binormals[i], normals[i], texturePoints[i], index);
                index++;
                if (ii % MeridiansCount == 1) //okrążyliśmy całą kulę
                    ii -= MeridiansCount;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(ii,
                    tangents[ii], binormals[ii], normals[ii], texturePoints[ii], index);
                index++;
            }

            TextureCoords[index - 1].X = 1f;  //jest to konieczne, to późniejszej poprawnej interpolacji, w ostatnim trójkącie ostatni wierzchołek musi mieć współrzedną X równą 1f, a nie 0f (ten sam wierzchołek w pierwszym trójkącie ma 0f)

            for (int i=1; i<ParallelsCount - 2; i++)
                //i jest indeksem górnego równoleżnika kwadracików, które dzielimy na trójkąty
                //rozważamy kwadraciki między równoleżnikami o indeksach i oraz i+1
            {
                int upperRightSquareVertexIndex = 1 + (i - 1) * MeridiansCount;
                int upperLeftSquareVertexIndex = upperRightSquareVertexIndex + 1;
                int lowerRightSquareVertexIndex = 1 + i * MeridiansCount;
                int lowerLeftSquareVertexIndex = lowerRightSquareVertexIndex + 1;
                for (int j=0; j<MeridiansCount; j++)
                {
                    //Trzy pierwsze wywołania funkcji Put... to pierwszy trójkąt z kwadracika
                    //Dbam oczywiście o kolejność clockwise
                    //Trzy kolejne wywołania funkcji Put... to drugi trójkąt z kwadracika
                    PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                        upperRightSquareVertexIndex,
                        tangents[upperRightSquareVertexIndex],
                        binormals[upperRightSquareVertexIndex],
                        normals[upperRightSquareVertexIndex], 
                        texturePoints[upperRightSquareVertexIndex], index);
                    index++;
                    PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                        lowerRightSquareVertexIndex,
                        tangents[lowerRightSquareVertexIndex],
                        binormals[lowerRightSquareVertexIndex],
                        normals[lowerRightSquareVertexIndex],
                        texturePoints[lowerRightSquareVertexIndex],
                        index);
                    index++;
                    if (lowerLeftSquareVertexIndex % MeridiansCount == 1)
                    {
                        //okrążyliśmy całą kulę i musimy zrobić cyklicznie (tzn. prawy kraniec kwadratu wskazuje ostatni południk, więc lewy musi wskazywać od nowa na punkt na pierwszym z rozważanych w danym równoleżniku)
                        lowerLeftSquareVertexIndex -= MeridiansCount;
                        upperLeftSquareVertexIndex -= MeridiansCount;
                    }
                    PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                        upperLeftSquareVertexIndex,
                        tangents[upperLeftSquareVertexIndex],
                        binormals[upperLeftSquareVertexIndex],
                        normals[upperLeftSquareVertexIndex],
                        texturePoints[upperLeftSquareVertexIndex],
                        index);
                    index++;
                    PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                        upperLeftSquareVertexIndex,
                        tangents[upperLeftSquareVertexIndex],
                        binormals[upperLeftSquareVertexIndex],
                        normals[upperLeftSquareVertexIndex],
                        texturePoints[upperLeftSquareVertexIndex],
                        index);
                    index++;
                    PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                        lowerRightSquareVertexIndex,
                        tangents[lowerRightSquareVertexIndex],
                        binormals[lowerRightSquareVertexIndex],
                        normals[lowerRightSquareVertexIndex],
                        texturePoints[lowerRightSquareVertexIndex],
                        index);
                    index++;
                    PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                        lowerLeftSquareVertexIndex,
                        tangents[lowerLeftSquareVertexIndex],
                        binormals[lowerLeftSquareVertexIndex],
                        normals[lowerLeftSquareVertexIndex],
                        texturePoints[lowerLeftSquareVertexIndex],
                        index);
                    index++;


                    upperRightSquareVertexIndex++;
                    lowerRightSquareVertexIndex++;
                    lowerLeftSquareVertexIndex++;
                    upperLeftSquareVertexIndex++;
                }
                TextureCoords[index - 4].X = 1f ;
                TextureCoords[index - 3].X = 1f;
                TextureCoords[index - 1].X = 1f;
            }
            for (int i = pointsCount-1-MeridiansCount, ii = i+1; 
                i < pointsCount-1; 
                i++, ii++)
            {
                //tworzymy trójkąt z biegunu północnego oraz dwóch na równoleżniku poniżej
                //dbam o kolejność clockwise
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(i, tangents[i],
                    binormals[i], normals[i], texturePoints[i], index);
                index++;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(
                    pointsCount - 1, tangents[pointsCount - 1],
                    binormals[pointsCount - 1], normals[pointsCount - 1], 
                    new PointF(texturePoints[i].X, 0f), index);
                index++;
                if (ii % MeridiansCount == 1) //okrążyliśmy całą kulę
                    ii -= MeridiansCount;
                PutVertexAndCorrespondingAttributesIntoTriangleArrays(ii,
                    tangents[ii], binormals[ii], normals[ii], 
                    texturePoints[ii], index);
                index++;
            }
            TextureCoords[index - 1].X = 1f;
        }



    
        private int CountTriangles()
        {
            return 2 * MeridiansCount   //trójkąty na biegunach o wierzchołkach w biegunach
                + 2 * MeridiansCount * (ParallelsCount - 3);   //każdy kwadrat siatki składa się z 2 trójkątów
        }


    }
}

