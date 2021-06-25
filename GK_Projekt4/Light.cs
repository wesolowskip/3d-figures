using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    [Serializable]
    class Light : INameable
    {
        private Color _id = Color.White;
        private Color _is = Color.White;

        public Light(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            UpdateNormalizedId();
            UpdateNormalizedIs();
        }

        [Browsable(false)]
        public static double[] NormalizedIa = new double[] { 0.2d, 0.2d, 0.2d };  //składowa ambient światła
        [Description("Kolor światła rozproszonego")]
        public Color Id { get => _id; set { _id = value; UpdateNormalizedId(); } }
        [Description("Kolor światła odbić lustrzanych")]
        public Color Is { get => _is; set { _is = value; UpdateNormalizedIs(); } }
        [Description("Współczynnik w atenuacji danej wzorem 1/(Ac + dist * Al + dist^2 * Aq)")]
        public double Ac { get; set; } = 1d;
        [Description("Współczynnik w atenuacji danej wzorem 1/(Ac + dist * Al + dist^2 * Aq)")]
        public double Aq { get; set; } = 0.000001d;
        [Description("Współczynnik w atenuacji danej wzorem 1/(Ac + dist * Al + dist^2 * Aq)")]
        public double Al { get; set; } = 0.000009d;
        [Description("Współrzędna x światła")]
        public int X { get; set; }
        [Description("Współrzędna y światła")]
        public int Y { get; set; }
        [Description("Współrzędna z światła")]
        public int Z { get; set; }
        [Description("Nazwa światła")]
        public string Name { get; } = "Światło";
        public override string ToString() => Name;

        [Browsable(false)]
        public double[] NormalizedId = new double[3];
        [Browsable(false)]
        public double[] NormalizedIs = new double[3];

        private void UpdateNormalizedId()
        {
            const double tmp = 1d / 255;
            NormalizedId[0] = Id.R * tmp;
            NormalizedId[1] = Id.G * tmp;
            NormalizedId[2] = Id.B * tmp;
        }
        private void UpdateNormalizedIs()
        {
            const double tmp = 1d / 255;
            NormalizedIs[0] = Is.R * tmp;
            NormalizedIs[1] = Is.G * tmp;
            NormalizedIs[2] = Is.B * tmp;
        }
    }
}
