using System;
using System.Drawing;
using System.Windows.Forms;

namespace APO_PJ_AP
{
    public partial class SkalaHSV : Form
    {
        public SkalaHSV()
        {
            InitializeComponent();
        }

        private void kolor(Panel pn)
        {
            ColorDialog cld = new ColorDialog();
            Panel tmp = new Panel();
            cld.AllowFullOpen = true;
            cld.Color = tmp.BackColor;
            cld.FullOpen = true;

            if (cld.ShowDialog() == DialogResult.OK)
            {
                tmp.BackColor = cld.Color;
                pn.BackColor = cld.Color;

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            kolor(pnlDarkColour2);
            Color original = pnlDarkColour2.BackColor;
            // Color original = Color.FromArgb(50, 120, 200);
            // original = {Name=ff3278c8, ARGB=(255, 50, 120, 200)}

            double hue;
            double saturation;
            double value;
            ColorToHSV(original, out hue, out saturation, out value);
            // hue        = 212.0
            // saturation = 0.75
            // value      = 0.78431372549019607

            Color copy = ColorFromHSV(hue, saturation, value);
            // copy = {Name=ff3278c8, ARGB=(255, 50, 120, 200)}

            // Compare that to the HSL values that the .NET framework provides: 
            original.GetHue();        // 212.0
            original.GetSaturation(); // 0.6
            original.GetBrightness(); // 0.490196079

            double h;
            double s;
            double v;

            double r;
            double g;
            double b;

            h = original.GetHue();
            s = original.GetSaturation();
            v = original.GetBrightness();
            //     label1.Text = "H = " + h.ToString() + " S = " + s.ToString() + " V = " + v.ToString();
            labelH.Text = h.ToString();
            labelS.Text = s.ToString();
            labelV.Text = v.ToString();


            r = original.R;
            g = original.G;
            b = original.B;

            labelR.Text = r.ToString();
            labelG.Text = g.ToString();
            labelB.Text = b.ToString();


        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            return Color.FromArgb(255, v, p, q);
        }
    }
}
