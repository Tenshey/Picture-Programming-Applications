using System;
using System.Drawing;
using System.Windows.Forms;
using APO_PJ_AP.Skale;

namespace APO_PJ_AP
{
    public partial class SkalaRGB : Form
    {
        private bool updating;

        public SkalaRGB()
        {
            InitializeComponent();
        }

        #region Handlers
        private void RGBValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                preview.BackColor = Color.FromArgb((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);
                hexBox.Text = ColorSpaceHelper.RGBToHex((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);

                updating = true;
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void HSLValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSLtoRGB((double)hueUD.Value, (double)satUD.Value / 100.0, (double)lumUD.Value / 100.0));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void HSBValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSBtoRGB((double)hUD.Value, (double)sUD.Value / 100.0, (double)bUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void CMYKValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.CMYKtoRGB((double)cyanUD.Value / 100.0, (double)magentaUD.Value / 100.0, (double)yellowUD.Value / 100.0, (double)blackUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void YUVValueChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.YUVtoRGB((double)yUD.Value / 100.0, (-0.436 + ((double)uUD.Value / 100.0)), (-0.615 + ((double)vUD.Value / 100.0))));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        #endregion

        #region Updates
        private void UpdateRGB(RGB rgb)
        {
            if (Convert.ToInt32(redUD.Value) != rgb.Red) redUD.Value = rgb.Red;
            if (Convert.ToInt32(greenUD.Value) != rgb.Green) greenUD.Value = rgb.Green;
            if (Convert.ToInt32(blueUD.Value) != rgb.Blue) blueUD.Value = rgb.Blue;

            preview.BackColor = Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
            hexBox.Text = ColorSpaceHelper.RGBToHex(rgb.Red, rgb.Green, rgb.Blue);
        }

        private void UpdateHSL(HSL hsl)
        {
            if (Convert.ToInt32(hsl.Hue) != (int)hueUD.Value) hueUD.Value = Convert.ToInt32(hsl.Hue);
            if (Convert.ToInt32(hsl.Saturation * 100) != (int)satUD.Value) satUD.Value = Convert.ToInt32(hsl.Saturation * 100);
            if (Convert.ToInt32(hsl.Luminance * 100) != (int)lumUD.Value) lumUD.Value = Convert.ToInt32(hsl.Luminance * 100);
        }

        private void UpdateHSB(HSB hsb)
        {
            if (Convert.ToInt32(hsb.Hue) != (int)hUD.Value) hUD.Value = Convert.ToInt32(hsb.Hue);
            if (Convert.ToInt32(hsb.Saturation * 100) != (int)sUD.Value) sUD.Value = Convert.ToInt32(hsb.Saturation * 100);
            if (Convert.ToInt32(hsb.Brightness * 100) != (int)bUD.Value) bUD.Value = Convert.ToInt32(hsb.Brightness * 100);
        }

        private void UpdateCMYK(CMYK cmyk)
        {
            if (Convert.ToInt32(cmyk.Cyan * 100) != (int)cyanUD.Value) cyanUD.Value = Convert.ToInt32(cmyk.Cyan * 100);
            if (Convert.ToInt32(cmyk.Magenta * 100) != (int)magentaUD.Value) magentaUD.Value = Convert.ToInt32(cmyk.Magenta * 100);
            if (Convert.ToInt32(cmyk.Yellow * 100) != (int)yellowUD.Value) yellowUD.Value = Convert.ToInt32(cmyk.Yellow * 100);
            if (Convert.ToInt32(cmyk.Black * 100) != (int)blackUD.Value) blackUD.Value = Convert.ToInt32(cmyk.Black * 100);
        }

        private void UpdateYUV(YUV yuv)
        {
            if (Convert.ToInt32(yuv.Y * 100) != (int)yUD.Value) yUD.Value = Convert.ToInt32(yuv.Y * 100);
            if (Convert.ToInt32((yuv.U + 0.436) * 100) != (int)uUD.Value) uUD.Value = Convert.ToInt32((yuv.U + 0.436) * 100);
            if (Convert.ToInt32((yuv.V + 0.615) * 100) != (int)vUD.Value) vUD.Value = Convert.ToInt32((yuv.V + 0.615) * 100);
        }

        #endregion

        private void redUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                preview.BackColor = Color.FromArgb((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);
                hexBox.Text = ColorSpaceHelper.RGBToHex((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);

                updating = true;
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }

        }

        private void greenUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                preview.BackColor = Color.FromArgb((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);
                hexBox.Text = ColorSpaceHelper.RGBToHex((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);

                updating = true;
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void blueUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                preview.BackColor = Color.FromArgb((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);
                hexBox.Text = ColorSpaceHelper.RGBToHex((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value);

                updating = true;
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void hueUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSLtoRGB((double)hueUD.Value, (double)satUD.Value / 100.0, (double)lumUD.Value / 100.0));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void satUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSLtoRGB((double)hueUD.Value, (double)satUD.Value / 100.0, (double)lumUD.Value / 100.0));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void lumUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSLtoRGB((double)hueUD.Value, (double)satUD.Value / 100.0, (double)lumUD.Value / 100.0));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void hUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSBtoRGB((double)hUD.Value, (double)sUD.Value / 100.0, (double)bUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void sUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSBtoRGB((double)hUD.Value, (double)sUD.Value / 100.0, (double)bUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void bUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.HSBtoRGB((double)hUD.Value, (double)sUD.Value / 100.0, (double)bUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void cyanUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.CMYKtoRGB((double)cyanUD.Value / 100.0, (double)magentaUD.Value / 100.0, (double)yellowUD.Value / 100.0, (double)blackUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void magentaUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.CMYKtoRGB((double)cyanUD.Value / 100.0, (double)magentaUD.Value / 100.0, (double)yellowUD.Value / 100.0, (double)blackUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void yellowUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.CMYKtoRGB((double)cyanUD.Value / 100.0, (double)magentaUD.Value / 100.0, (double)yellowUD.Value / 100.0, (double)blackUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void blackUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.CMYKtoRGB((double)cyanUD.Value / 100.0, (double)magentaUD.Value / 100.0, (double)yellowUD.Value / 100.0, (double)blackUD.Value / 100.0));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void yUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.YUVtoRGB((double)yUD.Value / 100.0, (-0.436 + ((double)uUD.Value / 100.0)), (-0.615 + ((double)vUD.Value / 100.0))));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void uUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.YUVtoRGB((double)yUD.Value / 100.0, (-0.436 + ((double)uUD.Value / 100.0)), (-0.615 + ((double)vUD.Value / 100.0))));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void vUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                UpdateRGB(ColorSpaceHelper.YUVtoRGB((double)yUD.Value / 100.0, (-0.436 + ((double)uUD.Value / 100.0)), (-0.615 + ((double)vUD.Value / 100.0))));
                UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
                updating = false;
            }
        }

        private void SkalaRGB_Load_1(object sender, EventArgs e)
        {
            updating = true;
            UpdateHSL(ColorSpaceHelper.RGBtoHSL((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
            UpdateHSB(ColorSpaceHelper.RGBtoHSB((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
            UpdateCMYK(ColorSpaceHelper.RGBtoCMYK((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
            UpdateYUV(ColorSpaceHelper.RGBtoYUV((int)redUD.Value, (int)greenUD.Value, (int)blueUD.Value));
            updating = false;
        }
    }
}

