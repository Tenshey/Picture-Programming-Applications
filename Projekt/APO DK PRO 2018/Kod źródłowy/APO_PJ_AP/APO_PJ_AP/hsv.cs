using System;
using System.Drawing;

namespace APO_PJ_AP
{
    public static class hsv
    {
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

    /*
    public static RGBtoHSV(int red, int green, int blue)
{
    // normalize red, green and blue values
    double r = ((double)red/255.0);
    double g = ((double)green/255.0);
    double b = ((double)blue/255.0);

    // conversion start
    double max = Math.Max(r, Math.Max(g, b));
    double min = Math.Min(r, Math.Min(g, b));

    double h = 0.0;
    if(max==r && g>=b)
    {
        h = 60 * (g-b)/(max-min);
    }
    else if(max==r && g < b)
    {
        h = 60 * (g-b)/(max-min) + 360;
    }
    else if(max == g)
    {
        h = 60 * (b-r)/(max-min) + 120;
    }
    else if(max == b)
    {
        h = 60 * (r-g)/(max-min) + 240;
    }

    double s = (max == 0)? 0.0 : (1.0 - (min/max));

    return (h, s, (double)max);
}
     */

}
