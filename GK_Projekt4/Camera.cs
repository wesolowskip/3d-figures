using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    [Serializable]
    class Camera: INameable
    {
        const double Precision = 0.001d;

        private int _n;
        private int _f;

        [Description("Pole widzenia kamery w stopniach")]
        public int Fov { get; set; }
        [Description("Przednia płaszczyzna obcinania")]
        public int N
        {
            get => _n; set
            {
                if (value <= 0 || value >= _f)
                    throw new ArgumentException("Musi być 0<N<F");
                _n = value;
            }
        }
        [Description("Tylna płaszczyzna obcinania")]
        public int F
        {
            get => _f; set
            {
                if (value<=_n)
                    throw new ArgumentException("Musi być 0<N<F");
                _f = value;
            }
        }
        double[] T { get; }   //target
        [Browsable(false)]
        public double[] P { get; }   //pozycja kamery
        double[] Uworld { get; } = new double[] { 0d, 1d, 0d };
        double[] U { get; set; } = new double[3];
        double[] R { get; set; } = new double[3];
        double[] D { get; set; } = new double[3];
        [Description("Nazwa kamery")]
        public string Name { get; } = "Kamera";

        public Camera(int fov, int n, int f, double[] T, double[] P)
        {
            Fov = fov;
            //Zakładam, że n i f podane w konstruktorze są poprawne,
            //ustawiam te wartości bez walidacji (gdybym się zdecydował na walidację
            //musiałbym pamiętać, że domyślnie jest N=F=0 i podczas walidacji jednej
            //wartości druga będzie równa 0)
            _n = n;
            _f = f;
            this.T = T;
            this.P = P;
            UpdateVectors();
        }
        private void UpdateVectors()
        {
            for (int i = 0; i < 3; i++)
                D[i] = T[i] - P[i];
            Utils.NormalizeVector(D);
            R = Utils.CrossProduct(Uworld, D);
            Utils.NormalizeVector(R);
            U = Utils.CrossProduct(D, R);
            Utils.NormalizeVector(D);
        }
        public double[,] GetViewMatrix()
        {
            var m1 = new double[4, 4];
            var m2 = new double[4, 4];
            for (int i = 0; i < 3; i++)
            {
                m1[0, i] = R[i];
                m1[1, i] = U[i];
                m1[2, i] = D[i];
                m2[i, 3] = -P[i];
            }
            m1[3, 3] = 1;
            for (int i = 0; i < 4; i++)
                m2[i, i] = 1;
            return Utils.MultiplyMatrices(m1, m2);
        }
        public double[,] GetProjectionMatrix(double aspect)
        {
            var result = new double[4, 4];
            double fovRadians = Fov * Math.PI / 180d;
            var ctg = Math.Cos(fovRadians / 2) / Math.Sin(fovRadians / 2);
            result[0, 0] = ctg / aspect;
            result[1, 1] = ctg;
            result[2, 2] = (double)(F + N) / (F - N);   //bardzo istotne rzutowanie! (nie mogłem znaleźć błędu)
            result[3, 2] = 1d;
            result[2, 3] = -2d * F * N / (F - N);  //bardzo istotne jest by obliczenia robily sie na double a nie na int
            return result;
        }
        public override string ToString() => Name;
        public void Translate(int forward, int right, int up)
        {
            for (int i = 0; i < 3; i++)
            {
                double offset = -forward * D[i] + right*R[i] + up*U[i];   //ruch w prawo zrobiłem odwrotnie, by był bardziej intuicyjny
                T[i] += offset;
                P[i] += offset;
            }
            //Wektory D, R, U się nie zmieniają
        }
        public void ChangeDistanceFromTarget(int offset)
        {
            double xDiff = P[0] - T[0];
            double yDiff = P[1] - T[1];
            double zDiff = P[2] - T[2];

            double r = Math.Sqrt(xDiff * xDiff + zDiff * zDiff + yDiff * yDiff);
            double longtitude = Math.Acos(xDiff /
                Math.Sqrt(xDiff * xDiff + zDiff * zDiff)) * (zDiff < 0 ? -1 : 1);
            double lattitude = Math.Acos(yDiff / r);

            r += offset;
            if (r < Precision)
                r = Precision;

            double sinLong = Math.Sin(longtitude);
            double cosLong = Math.Cos(longtitude);
            double sinLat = Math.Sin(lattitude);
            double cosLat = Math.Cos(lattitude);

            P[0] = T[0] + r * sinLat * cosLong;
            P[1] = T[1] + r * cosLat;
            P[2] = T[2] + r * sinLat * sinLong;

            //wektory D, R, U się nie zmieniają
        }
        public void Rotate(double horAngle, double verAngle)
        {
            double xDiff = P[0] - T[0];
            double yDiff = P[1] - T[1];
            double zDiff = P[2] - T[2];

            double r = Math.Sqrt(xDiff * xDiff + zDiff * zDiff + yDiff * yDiff);
            double longtitude = Math.Acos(xDiff / 
                Math.Sqrt(xDiff * xDiff + zDiff * zDiff)) * (zDiff < 0 ? -1 : 1);
            double lattitude = Math.Acos(yDiff / r);

            lattitude += verAngle;
            if (lattitude > Math.PI - Precision)   //nie chcę kamery idealnie w pionie z targetem, bo wtedy w kamerze R=0 i U=0 i wyświetlane trójkąty są zdegenerowane
                lattitude = Math.PI - Precision;
            else if (lattitude < Precision)
                lattitude = Precision;
            longtitude += horAngle;

            double sinLong = Math.Sin(longtitude);
            double cosLong = Math.Cos(longtitude);
            double sinLat = Math.Sin(lattitude);
            double cosLat = Math.Cos(lattitude);

            P[0] = T[0] + r * sinLat * cosLong;
            P[1] = T[1] + r * cosLat;
            P[2] = T[2] + r * sinLat * sinLong;

            UpdateVectors();
        }
    }
}
