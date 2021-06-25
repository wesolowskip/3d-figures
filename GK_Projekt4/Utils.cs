using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    static class Utils
    {
        private const double Epsilon = 0.00001;
        public static double[] CrossProduct(double[] v1, double[] v2)
        {
            return new double[]
            {
                v1[1] * v2[2] - v1[2] * v2[1],
                v1[2] * v2[0] - v1[0] * v2[2],
                v1[0] * v2[1] - v1[1] * v2[0]
            };
        }
        public static double DotProduct(double[] v1, double[] v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }
        public static void NormalizeVector(double[] v)
        {
            double length = VectorLength(v);
            if (length < Epsilon)
                return;
            v[0] /= length;
            v[1] /= length;
            v[2] /= length;
        }
        public static void NormalizeVectors(double[,] vectors)
        {
            for (int i=0; i<vectors.GetLength(1); i++)
            {
                double length = Math.Sqrt(vectors[0, i] * vectors[0, i] +
                    vectors[1, i] * vectors[1, i] + vectors[2, i] * vectors[2, i]);
                if (length < Epsilon)
                    continue;
                vectors[0, i] /= length;
                vectors[1, i] /= length;
                vectors[2, i] /= length;
            }
        }
        public static double VectorLength(double[] v)
        {
            return Math.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
        }
        public static double[,] MultiplyMatrices(params double[][,] matrices)
        {
            double[,] result = matrices[0];
            for (int i = 1; i < matrices.Length; i++)
                result = MultiplyTwoMatrices(result, matrices[i]);
            return result;
        }
        private static double[,] MultiplyTwoMatrices(double[,] m1, double[,] m2)
        {
            if (m1.GetLength(1) != m2.GetLength(0))
                throw new Exception("złe rozmiary");
            double[,] result = new double[m1.GetLength(0), m2.GetLength(1)];
            for (int i = 0; i < m1.GetLength(0); i++)
                for (int j = 0; j < m2.GetLength(1); j++)
                {
                    result[i, j] = 0f;
                    for (int k = 0; k < m1.GetLength(1); k++)
                        result[i, j] += m1[i, k] * m2[k, j];
                }

            return result;
        }
        public static double[,] Inverse3x3Matrix(double [,] matrix)
        {
            //https://stackoverflow.com/questions/983999/simple-3x3-matrix-inverse-code-c
            double det = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2]) -
                         matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                         matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

            double invDet = 1 / det;

            double[,] inverse = new double[3,3]; // inverse of matrix m
            inverse[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2]) * invDet;
            inverse[0, 1] = (matrix[0, 2] * matrix[2, 1] - matrix[0, 1] * matrix[2, 2]) * invDet;
            inverse[0, 2] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) * invDet;
            inverse[1, 0] = (matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2]) * invDet;
            inverse[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) * invDet;
            inverse[1, 2] = (matrix[1, 0] * matrix[0, 2] - matrix[0, 0] * matrix[1, 2]) * invDet;
            inverse[2, 0] = (matrix[1, 0] * matrix[2, 1] - matrix[2, 0] * matrix[1, 1]) * invDet;
            inverse[2, 1] = (matrix[2, 0] * matrix[0, 1] - matrix[0, 0] * matrix[2, 1]) * invDet;
            inverse[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1]) * invDet;
            return inverse;
        }
        public static double[] NormalizeToNDC(double [] clippingCoords)
        {
            double[] coords = new double[3];
            double w = clippingCoords[3];
            if (Math.Abs(w) >= Epsilon)
            {
                for (int i = 0; i < 3; i++)
                    coords[i] = clippingCoords[i] / w;
            }
            return coords;
        }

        public static double[,] Inverse4x4Matrix(double [,] matrix)
        {
        //https://stackoverflow.com/questions/1148309/inverting-a-4x4-matrix
            var inv = new double[4, 4];
            inv [0,0] = matrix [1,1]  * matrix [2,2] * matrix [3,3] - 
             matrix [1,1]  * matrix [2,3] * matrix [3,2] - 
             matrix [2,1]  * matrix [1,2]  * matrix [3,3] + 
             matrix [2,1]  * matrix [1,3]  * matrix [3,2] +
             matrix [3,1] * matrix [1,2]  * matrix [2,3] - 
             matrix [3,1] * matrix [1,3]  * matrix [2,2];

            inv [1,0] = -matrix [1,0]  * matrix [2,2] * matrix [3,3] + 
              matrix [1,0]  * matrix [2,3] * matrix [3,2] + 
              matrix [2,0]  * matrix [1,2]  * matrix [3,3] - 
              matrix [2,0]  * matrix [1,3]  * matrix [3,2] - 
              matrix [3,0] * matrix [1,2]  * matrix [2,3] + 
              matrix [3,0] * matrix [1,3]  * matrix [2,2];

            inv [2,0] = matrix [1,0]  * matrix [2,1] * matrix [3,3] - 
             matrix [1,0]  * matrix [2,3] * matrix [3,1] - 
             matrix [2,0]  * matrix [1,1] * matrix [3,3] + 
             matrix [2,0]  * matrix [1,3] * matrix [3,1] + 
             matrix [3,0] * matrix [1,1] * matrix [2,3] - 
             matrix [3,0] * matrix [1,3] * matrix [2,1];

            inv [3,0] = -matrix [1,0]  * matrix [2,1] * matrix [3,2] + 
               matrix [1,0]  * matrix [2,2] * matrix [3,1] +
               matrix [2,0]  * matrix [1,1] * matrix [3,2] - 
               matrix [2,0]  * matrix [1,2] * matrix [3,1] - 
               matrix [3,0] * matrix [1,1] * matrix [2,2] + 
               matrix [3,0] * matrix [1,2] * matrix [2,1];

            inv [0,1] = -matrix [0,1]  * matrix [2,2] * matrix [3,3] + 
              matrix [0,1]  * matrix [2,3] * matrix [3,2] + 
              matrix [2,1]  * matrix [0,2] * matrix [3,3] - 
              matrix [2,1]  * matrix [0,3] * matrix [3,2] - 
              matrix [3,1] * matrix [0,2] * matrix [2,3] + 
              matrix [3,1] * matrix [0,3] * matrix [2,2];

            inv [1,1] = matrix [0,0]  * matrix [2,2] * matrix [3,3] - 
             matrix [0,0]  * matrix [2,3] * matrix [3,2] - 
             matrix [2,0]  * matrix [0,2] * matrix [3,3] + 
             matrix [2,0]  * matrix [0,3] * matrix [3,2] + 
             matrix [3,0] * matrix [0,2] * matrix [2,3] - 
             matrix [3,0] * matrix [0,3] * matrix [2,2];

            inv [2,1] = -matrix [0,0]  * matrix [2,1] * matrix [3,3] + 
              matrix [0,0]  * matrix [2,3] * matrix [3,1] + 
              matrix [2,0]  * matrix [0,1] * matrix [3,3] - 
              matrix [2,0]  * matrix [0,3] * matrix [3,1] - 
              matrix [3,0] * matrix [0,1] * matrix [2,3] + 
              matrix [3,0] * matrix [0,3] * matrix [2,1];

            inv [3,1] = matrix [0,0]  * matrix [2,1] * matrix [3,2] - 
              matrix [0,0]  * matrix [2,2] * matrix [3,1] - 
              matrix [2,0]  * matrix [0,1] * matrix [3,2] + 
              matrix [2,0]  * matrix [0,2] * matrix [3,1] + 
              matrix [3,0] * matrix [0,1] * matrix [2,2] - 
              matrix [3,0] * matrix [0,2] * matrix [2,1];

            inv [0,2] = matrix [0,1]  * matrix [1,2] * matrix [3,3] - 
             matrix [0,1]  * matrix [1,3] * matrix [3,2] - 
             matrix [1,1]  * matrix [0,2] * matrix [3,3] + 
             matrix [1,1]  * matrix [0,3] * matrix [3,2] + 
             matrix [3,1] * matrix [0,2] * matrix [1,3] - 
             matrix [3,1] * matrix [0,3] * matrix [1,2];

            inv [1,2] = -matrix [0,0]  * matrix [1,2] * matrix [3,3] + 
              matrix [0,0]  * matrix [1,3] * matrix [3,2] + 
              matrix [1,0]  * matrix [0,2] * matrix [3,3] - 
              matrix [1,0]  * matrix [0,3] * matrix [3,2] - 
              matrix [3,0] * matrix [0,2] * matrix [1,3] + 
              matrix [3,0] * matrix [0,3] * matrix [1,2];

            inv [2,2] = matrix [0,0]  * matrix [1,1] * matrix [3,3] - 
              matrix [0,0]  * matrix [1,3] * matrix [3,1] - 
              matrix [1,0]  * matrix [0,1] * matrix [3,3] + 
              matrix [1,0]  * matrix [0,3] * matrix [3,1] + 
              matrix [3,0] * matrix [0,1] * matrix [1,3] - 
              matrix [3,0] * matrix [0,3] * matrix [1,1];

            inv [3,2] = -matrix [0,0]  * matrix [1,1] * matrix [3,2] + 
               matrix [0,0]  * matrix [1,2] * matrix [3,1] + 
               matrix [1,0]  * matrix [0,1] * matrix [3,2] - 
               matrix [1,0]  * matrix [0,2] * matrix [3,1] - 
               matrix [3,0] * matrix [0,1] * matrix [1,2] + 
               matrix [3,0] * matrix [0,2] * matrix [1,1];

            inv [0,3] = -matrix [0,1] * matrix [1,2] * matrix [2,3] + 
              matrix [0,1] * matrix [1,3] * matrix [2,2] + 
              matrix [1,1] * matrix [0,2] * matrix [2,3] - 
              matrix [1,1] * matrix [0,3] * matrix [2,2] - 
              matrix [2,1] * matrix [0,2] * matrix [1,3] + 
              matrix [2,1] * matrix [0,3] * matrix [1,2];

            inv [1,3] = matrix [0,0] * matrix [1,2] * matrix [2,3] - 
             matrix [0,0] * matrix [1,3] * matrix [2,2] - 
             matrix [1,0] * matrix [0,2] * matrix [2,3] + 
             matrix [1,0] * matrix [0,3] * matrix [2,2] + 
             matrix [2,0] * matrix [0,2] * matrix [1,3] - 
             matrix [2,0] * matrix [0,3] * matrix [1,2];

            inv [2,3] = -matrix [0,0] * matrix [1,1] * matrix [2,3] + 
               matrix [0,0] * matrix [1,3] * matrix [2,1] + 
               matrix [1,0] * matrix [0,1] * matrix [2,3] - 
               matrix [1,0] * matrix [0,3] * matrix [2,1] - 
               matrix [2,0] * matrix [0,1] * matrix [1,3] + 
               matrix [2,0] * matrix [0,3] * matrix [1,1];

            inv [3,3] = matrix [0,0] * matrix [1,1] * matrix [2,2] - 
              matrix [0,0] * matrix [1,2] * matrix [2,1] - 
              matrix [1,0] * matrix [0,1] * matrix [2,2] + 
              matrix [1,0] * matrix [0,2] * matrix [2,1] + 
              matrix [2,0] * matrix [0,1] * matrix [1,2] - 
              matrix [2,0] * matrix [0,2] * matrix [1,1];

            double det = matrix [0,0] * inv [0,0] + matrix [0,1] * inv [1,0] + matrix [0,2] * inv [2,0] + matrix [0,3] * inv [3,0];

            if (Math.Abs(det) < Epsilon)
                throw new ArgumentException("Macierz nieodwracalna");

            det = 1d / det;

            for (int i = 0; i < 4; i++)
                for (int j=0; j<4; j++)
                    inv[i, j] *= det;
            return inv;
        }
        public static double[,] TransposeMatrix(double [,] matrix)
        {
            double[,] result = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[j, i] = matrix[i, j];
            return result;
        }
        public static T[] Permute<T>(this T[] array, int[] order)
        {
            T[] result = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[order[i]];
            return result;
        }
        public static double[] MultiplyVector(double multiply, double[] vector)
        {
            double[] result = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
                result[i] = multiply * vector[i];
            return result;
        }
        public static double[] SubtractVectors(double[] v1, double[] v2)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
                result[i] = v1[i] - v2[i];
            return result;
        }
        public static double[] AddVectors(double[] v1, double[] v2)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
                result[i] = v1[i] + v2[i];
            return result;
        }
        public static double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
        {
            int m = matrix.GetLength(0);
            int n = matrix.GetLength(1);
            double[] result = new double[n];  //zainicjowane zerami
            for (int i=0; i<m; i++)
            {
                for (int j = 0; j < n; j++)
                    result[i] += matrix[i, j] * vector[j];
            }
            return result;
        }
    }
}
