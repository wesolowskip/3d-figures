using GK_Projekt4.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    static class SutherlandHodgman
    {
        public static List<Triangle> ClipTriangles(double[,] pointCoords, Figure figure)
        {
            var triangles = figure.Triangles;
            var worldPointsCoords = figure.WorldPointsCoords;
            var worldNormals = figure.WorldNormals;
            var worldBinormals = figure.WorldBinormals;
            var worldTangents = figure.WorldTangents;
            var textureCoords = figure.TextureCoords;

            List<Triangle> result = new List<Triangle>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                //obcinamy trójkąt o numerze i/3 (licząc od 0)
                //ma on wierzchołki w kolumnach pointCoords o indeksach
                //triangles[i], triangles[i+1], triangles[i+2]
                List<double[]> clippedPoints = new List<double[]>();
                List<double[]> worldCoords = new List<double[]>();
                //Lista interpolatedVectors będzie zawierała wektory, które
                //interpolujemy
                //W szczególności, interpolatedVectors[i][j] będzie określać
                //dla i-tego punktu:
                //- wektor normalny dla j=0
                //- wektor binormalny dla j=1
                //- wektor styczny dla j=2
                //Oczywiście interpolatedVectors[i][j] jest 3-elementową tablicą
                //określającą x,y,z wektora
                var vectors = new List<double[][]>();
                var texturePoints = new List<PointF>();
                for (int j = i; j < i + 3; j++)
                {
                    clippedPoints.Add(
                        new double[]
                        {
                            pointCoords[0, triangles[j]],
                            pointCoords[1, triangles[j]],
                            pointCoords[2, triangles[j]],
                            pointCoords[3, triangles[j]]
                        });
                    texturePoints.Add(textureCoords[j]);
                    worldCoords.Add(
                            new double[]   //współrzędne w przestrzeni świata
                            {
                                worldPointsCoords[0, triangles[j]],
                                worldPointsCoords[1, triangles[j]],
                                worldPointsCoords[2, triangles[j]],
                            });
                    vectors.Add(
                        new double[3][]
                        {
  
                            new double[]  //współrzędne wektora normalnego
                            {
                                worldNormals[0, j],
                                worldNormals[1,j],
                                worldNormals[2,j]
                            },
                            new double[]  //współrzedne wektora binormalnego
                            {
                                worldBinormals[0, j],
                                worldBinormals[1,j],
                                worldBinormals[2,j]
                            },
                            new double[]  //współrzędne wektora stycznego
                            {
                                worldTangents[0,j],
                                worldTangents[1,j],
                                worldTangents[2,j]
                            }
                        }
                        );

                }
                //Implementacja obcinania dokładnie taka jak na slajdach
                for (int j = 0; j < 6; j++)
                {
                    //obcinamy względem j-tej płaszczyzny
                    double[] s, p, intersection, intersectionWorldCoords, sWorldCoords, pWorldCoords;
                    double[][] intersectionVectors, sVectors,
                        pVectors;
                    PointF sTextureCoords, pTextureCoords, intersectionTextureCoords;

                    var newClippedPoints = new List<double[]>();
                    var newWorldCoords = new List<double[]>();
                    var newVectors = new List<double[][]>();
                    var newTexturePoints = new List<PointF>();

                    s = clippedPoints[clippedPoints.Count - 1];
                    sWorldCoords = worldCoords[clippedPoints.Count - 1];
                    sVectors = vectors[clippedPoints.Count - 1];
                    sTextureCoords = texturePoints[clippedPoints.Count - 1];

                    for (int k = 0; k < clippedPoints.Count; k++)
                    {
                        p = clippedPoints[k];
                        pVectors = vectors[k];
                        pWorldCoords = worldCoords[k];
                        pTextureCoords = texturePoints[k];
                        if (IsInside(p, j))
                        {
                            if (IsInside(s, j))
                            {
                                newClippedPoints.Add(p);
                                newWorldCoords.Add(pWorldCoords);
                                newVectors.Add(pVectors);
                                newTexturePoints.Add(pTextureCoords);
                            }
                            else
                            {
                                (intersection, intersectionWorldCoords,
                                    intersectionVectors, intersectionTextureCoords) =
                                    Intersect(s, p, sWorldCoords, pWorldCoords,
                                    sVectors, pVectors, sTextureCoords, pTextureCoords, j);

                                newClippedPoints.Add(intersection);
                                newWorldCoords.Add(intersectionWorldCoords);
                                newVectors.Add(intersectionVectors);
                                newTexturePoints.Add(intersectionTextureCoords);

                                newClippedPoints.Add(p);
                                newWorldCoords.Add(pWorldCoords);
                                newVectors.Add(pVectors);
                                newTexturePoints.Add(pTextureCoords);
                            }
                        }
                        else if (IsInside(s, j))
                        {
                            (intersection, intersectionWorldCoords,
                                intersectionVectors, intersectionTextureCoords) =
                                Intersect(s, p, sWorldCoords, pWorldCoords,
                                sVectors, pVectors, sTextureCoords, pTextureCoords, j);

                            newClippedPoints.Add(intersection);
                            newWorldCoords.Add(intersectionWorldCoords);
                            newVectors.Add(intersectionVectors);
                            newTexturePoints.Add(intersectionTextureCoords);
                        }
                        s = p;
                        sWorldCoords = pWorldCoords;
                        sVectors = pVectors;
                        sTextureCoords = pTextureCoords;
                    }
                    clippedPoints = newClippedPoints;
                    worldCoords = newWorldCoords;
                    vectors = newVectors;
                    texturePoints = newTexturePoints;
                    if (clippedPoints.Count == 0)   //BARDZO WAŻNE!!! 
                        break;
                }
                //W tym momencie clippedPoints zawiera punkty
                //obciętej figury nr i
                //Teraz dzielimy obcięty wielokąt na trójkąty
                for (int j = 1; j < clippedPoints.Count - 1; j++)
                    result.Add(
                        new Triangle(
                            new double[][] { clippedPoints[0], clippedPoints[j], clippedPoints[j + 1] },  //współrzędne w przestrzeni obcinania
                            new double[][] { worldCoords[0], worldCoords[j], worldCoords[j+1] },  //współrzędne w przestrzeni świata punktów o indeksach 0,j,j+1 
                            new double[][] { vectors[0][0], vectors[j][0], vectors[j + 1][0] },  //współrzędne wektorów normalnych punktów o indeksach 0, j, j+1
                            new double[][] { vectors[0][1], vectors[j][1], vectors[j + 1][1] },  //współrzędne wektorów binormalnych ...
                            new double[][] { vectors[0][2], vectors[j][2], vectors[j + 1][2] },  //współrzędne wektorów stycznych ...
                            new PointF[] { texturePoints[0], texturePoints[j], texturePoints[j+1] },
                            figure)
                            );
            }
            return result;
        }

        private static (double[] point, double[] pointWorldCoords,
            double[][] pointVectors, PointF pointTextureCoords) 
            Intersect(double[] a, double[] b, double[] aWorldCoords, double[] bWorldCoords,
            double[][] aVectors, double[][] bVectors, PointF aTextureCoords, PointF bTextureCoords,
            int plane_index)
        {
            double da = Dist(a, plane_index);
            double db = Dist(b, plane_index);
            double dc = da / (da - db);
            var c = new double[4];
            for (int i = 0; i < 4; i++)
                c[i] = (1 - dc) * a[i] + dc * b[i];
            //Mamy wyliczone współrzędne punktu przecięcia (c) w przestrzeni obcinania
            //Teraz zinterpolujemy inne atrybuty punktu c, jak np. współrzędną świata
            //oraz jego wektory
            var cTextureCoords = new PointF((float)((1-dc)*aTextureCoords.X + dc*bTextureCoords.X), 
                (float)((1-dc)*aTextureCoords.Y + dc*bTextureCoords.Y));
            var cWorldCoords = new double[3];
            for (int i = 0; i < 3; i++)
                cWorldCoords[i] = (1 - dc) * aWorldCoords[i] + dc * bWorldCoords[i];

            var cVectors = new double[aVectors.Length][];
            for (int i = 0; i < cVectors.Length; i++)
            {
                //interpolujemy i-ty element
                cVectors[i] = new double[3];
                for (int j = 0; j < 3; j++)   //zakładam, że interpolowane elementy mają 3 współrzędne x, y i z
                    cVectors[i][j] = (1 - dc) * aVectors[i][j] +
                        dc * bVectors[i][j];
                Utils.NormalizeVector(cVectors[i]);   //To chyba jest konieczne
            }
            return (c, cWorldCoords, cVectors, cTextureCoords);
        }

        private static bool IsInside(double[] coords, int plane_index)
        {
            return Dist(coords, plane_index) >= 0;
        }

        private static double Dist(double[] coords, int plane_index)
        {
            switch (plane_index)
            {
                case 0:
                    return coords[3] - coords[0];
                case 1:
                    return coords[3] + coords[0];
                case 2:
                    return coords[3] - coords[1];
                case 3:
                    return coords[3] + coords[1];
                case 4:
                    return coords[3] - coords[2];
                case 5:
                    return coords[3] + coords[2];
            }
            throw new ArgumentException("Niepoprawna wartość i");
        }
    }
}
