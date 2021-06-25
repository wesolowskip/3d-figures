using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    static class PhongLightingModel
    {
        public static Color CalculateColor(double[] ka, double[] ks, double[] kd, uint shininess, 
            double[] worldCoords, double[] normal, List<Light> lights, double[] cameraCoords)
        {
            double[] V = new double[] //wersor do obserwatora od piksela
            {
                        cameraCoords[0] - worldCoords[0],
                        cameraCoords[1] - worldCoords[1],
                        cameraCoords[2] - worldCoords[2]
            };
            Utils.NormalizeVector(V);
            double[] result = new double[3]
            {
                        ka[0] * Light.NormalizedIa[0],
                        ka[1] * Light.NormalizedIa[1],
                        ka[2] * Light.NormalizedIa[2]
            };

            for (int i = 0; i < lights.Count; i++)
            {
                double[] LI = new double[]  //wersor od piksela do światła
                {
                        lights[i].X - worldCoords[0],
                        lights[i].Y - worldCoords[1],
                        lights[i].Z - worldCoords[2]
                };
                double dist = Utils.VectorLength(LI);
                Utils.NormalizeVector(LI);
                double LI_dot_N = Utils.DotProduct(LI, normal);
                double[] R = Utils.SubtractVectors(Utils.MultiplyVector(
                    2 * LI_dot_N, normal), LI);
                Utils.NormalizeVector(R);

                //Math.Max wzorowałem na https://learnopengl.com/Lighting/Basic-Lighting
                double diff = Math.Max(0d, LI_dot_N);
                double spec = Math.Pow(
                    Math.Max(0d, Utils.DotProduct(R, V)), shininess);
                double If = 1d / (lights[i].Ac + dist * lights[i].Al +
                    dist * dist * lights[i].Aq);
                for (int j = 0; j < 3; j++)  //Dla każdej składowej R,G,B
                {
                    result[j] += (kd[j] * diff * lights[i].NormalizedId[j]
                        + ks[j] * spec * lights[i].NormalizedIs[j])
                        * If;
                }
            }

            for (int i = 0; i < 3; i++)
                result[i] *= 256d;
            return Color.FromArgb(
                Math.Max(Math.Min(255, (int)result[0]), 0),
                Math.Max(Math.Min(255, (int)result[1]), 0),
                Math.Max(Math.Min(255, (int)result[2]), 0)
                );
        }
    }
}
