using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace APO_PJ_AP.Operation
{
    class Jednopunktowe
    {

        private static int safety_check(int val)
        {
            #region safety_check
            if (val > 255)
                return 255;
            if (val < 0)
                return 0;
            return val;
            #endregion
        }

        private static double safety_check_hsv(double val)
        {
            #region safety_check_hsv
            if (val > 1)
                return 1;
            if (val < 0)
                return 0;
            return val;
            #endregion
        }

        public static Bitmap Negacja(Bitmap bmp)
        {
            //Przygotuj tablice LUT i pokaz ja na wykresie
            byte[] LUT = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if (i > 255)
                {
                    LUT[i] = 255;
                }
                else if (i < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(255 - i);
                }
                //wykres.Series[0].Points.Add(new DataPoint(i, LUT[i]));
            }
            Bitmap bitmap = (Bitmap)bmp.Clone();
            //Pobierz wartosc wszystkich punktow obrazu
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte[] byteBuffer = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];
            Marshal.Copy(bmpData.Scan0, byteBuffer, 0, byteBuffer.Length);

            for (int i = 0; i < byteBuffer.Length; i++)
            {
                byteBuffer[i] = LUT[byteBuffer[i]];
            }
            //Ustaw wartosc wszystkich punktow obrazu
            Marshal.Copy(byteBuffer, 0, bmpData.Scan0, byteBuffer.Length);
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        public static Bitmap NegacjaHSV(Bitmap sourceBitmap)
        {
            #region NegacjaHSV
            Bitmap bitmap = (Bitmap)sourceBitmap.Clone();
            Bitmap bmpHSV = new Bitmap(sourceBitmap);
            BitmapData data = bmpHSV.LockBits(new Rectangle(0, 0, bmpHSV.Width, bmpHSV.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    double h, s, v;
                    hsv.ColorToHSV(sourceBitmap.GetPixel(x, y), out h, out s, out v);
                    h = (180.0f + h) % 360.0f;
                    v = 1.0f - v;
                    bitmap.SetPixel(x, y, hsv.ColorFromHSV(h, s, v));
                }
            }
            bmpHSV.UnlockBits(data);
            return bitmap;

            #endregion
        }
        public static Bitmap ConvertToGrayscale(Bitmap sourceBitmap)
        {
            #region ConvertToGrayscaleUseHSV
            Bitmap bitmap = (Bitmap)sourceBitmap.Clone();
            Bitmap bmpHSV = new Bitmap(sourceBitmap);
            BitmapData data = bmpHSV.LockBits(new Rectangle(0, 0, bmpHSV.Width, bmpHSV.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    double h, s, v;
                    hsv.ColorToHSV(sourceBitmap.GetPixel(x, y), out h, out s, out v);
                    bitmap.SetPixel(x, y, hsv.ColorFromHSV(h, 0, v));
                }
            }
            bmpHSV.UnlockBits(data);
            return bitmap;

            #endregion
        }
        public static Bitmap Progowanie(Bitmap sourceBmp, int b)
        {
            #region Tabkica LUT - BINERYZACJA
            //Przygotuj tablice LUT i pokaz ja na wykresie
            //wykres.Series[0].Points.Clear();
            byte[] LUT = new byte[256];
            int a = b;
            //wykres.Titles[0].Text = "Tablica LUT, p1 = " + a.ToString("0");
            for (int i = 0; i < 256; i++)
            {
                if (i < a)
                {
                    LUT[i] = 0;
                }
                else LUT[i] = 255;

                //wykres.Series[0].Points.Add(new DataPoint(i, LUT[i]));
            }

            Bitmap bitmap = (Bitmap)sourceBmp.Clone();
            //Pobierz wartosc wszystkich punktow obrazu
            BitmapData bmpData = bitmap.LockBits( new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];
            Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);
            for (int i = 0; i < pixelValues.Length; i++)
            {
                pixelValues[i] = LUT[pixelValues[i]];
            }
            //Ustaw wartosc wszystkich punktow obrazu
            Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            bitmap.UnlockBits(bmpData);

            return Bitonal(bitmap, Color.Black, Color.White, b);

            #endregion


        }

        public static Bitmap Bitonal(Bitmap sourceBitmap, Color darkColor, Color lightColor, int threshold)
        {
            #region Bitonal - progowanie z kolorem
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                if (pixelBuffer[k] + pixelBuffer[k + 1] + pixelBuffer[k + 2] <= threshold)
                {
                    pixelBuffer[k] = darkColor.B;
                    pixelBuffer[k + 1] = darkColor.G;
                    pixelBuffer[k + 2] = darkColor.R;
                }
                else
                {
                    pixelBuffer[k] = lightColor.B;
                    pixelBuffer[k + 1] = lightColor.G;
                    pixelBuffer[k + 2] = lightColor.R;
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
            #endregion
        }

        public static Bitmap ProgowanieHSV(Bitmap sourceBitmap, int threshold)
        {
            #region ProgowanieHSV
            Bitmap bitmap = (Bitmap)sourceBitmap.Clone();
            Bitmap bmpHSV = new Bitmap(sourceBitmap);
            BitmapData data = bmpHSV.LockBits(new Rectangle(0, 0, bmpHSV.Width, bmpHSV.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            double d = (double)threshold / 255;
            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    double h, s, v;
                    hsv.ColorToHSV(sourceBitmap.GetPixel(x, y), out h, out s, out v);
                    if (v < d)
                    {
                        bitmap.SetPixel(x, y, hsv.ColorFromHSV(h, 0, 0));
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, hsv.ColorFromHSV(h, 0, 1));
                    }

                }
            }
            bmpHSV.UnlockBits(data);
            return bitmap;

            #endregion
        }

        public static Bitmap Jasnosc(Bitmap sourceBitmap, int b)
        {
            #region Tabkica LUT - JASNOŚĆ
            //Przygotuj tablice LUT i pokaz ja na wykresie
            //wykres.Series[0].Points.Clear();
            byte[] LUT = new byte[256];
            //wykres.Titles[0].Text = "Tablica LUT, p1 = " + b;
            for (int i = 0; i < 256; i++)
            {
                if ((b + i) > 255)
                {
                    LUT[i] = 255;
                }
                else if ((b + i) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(b + i);
                }
                //wykres.Series[0].Points.Add(new DataPoint(i, LUT[i]));
            }
            Bitmap bitmap = (Bitmap)sourceBitmap.Clone();
            //Pobierz wartosc wszystkich punktow obrazu
            BitmapData bmpData = bitmap.LockBits( new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];
            Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);
            //Zmien jasnosc kazdego punktu zgodnie z tablica LUT
            for (int i = 0; i < pixelValues.Length; i++)
            {
                pixelValues[i] = LUT[pixelValues[i]];
            }
            //Ustaw wartosc wszystkich punktow obrazu
            Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            bitmap.UnlockBits(bmpData);
            return bitmap;
            #endregion
        }

        public static Bitmap Jasnosc_hsv(Bitmap sourceBitmap, int threshold)
        {
            #region regulacja_jasnoscia_hsv
            Bitmap bitmap = (Bitmap)sourceBitmap.Clone();
            FastBitmap fobrazek_bitmapa = new FastBitmap(bitmap);
            fobrazek_bitmapa.LockImage();

            for (int x = 0; x < sourceBitmap.Width; x++)
            {
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    double h, s, v;
                    hsv.ColorToHSV(fobrazek_bitmapa.GetPixel(x, y), out h, out s, out v);
                    v = v + ((double)threshold / 255);
                    if (v < 0) v = 0.0f;
                    if (v > 1) v = 1.0f;
                    fobrazek_bitmapa.SetPixel(x, y, hsv.ColorFromHSV(h, s, v));

                }
            }
            fobrazek_bitmapa.UnlockImage();
            return bitmap;
            #endregion
        }

        public static Bitmap ProgowanieZPoziomemSzarosci(Bitmap bmp, int a, int b)
        {
            #region Tabkica LUT -  BINERYZACJA Z ZACHOWANIEM POZIOMÓW SZAROŚCI
            //Przygotuj tablice LUT i pokaz ja na wykresie
            byte[] LUT = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if (a < b)
                {
                    if (i < a)
                    {
                        LUT[i] = 0;
                    }
                    else if (i > b)
                    {
                        LUT[i] = 0;
                    }
                    else LUT[i] = (byte)i;
                }
                if (b < a)
                {
                    if (i < b)
                    {
                        LUT[i] = 0;
                    }
                    else if (i > a)
                    {
                        LUT[i] = 0;
                    }
                    else LUT[i] = (byte)i;
                }


            }

            Bitmap bitmap = (Bitmap)bmp.Clone();
            //Pobierz wartosc wszystkich punktow obrazu
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];
            Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);
            //Korekcja gamma dla kazdego punktu zgodnie z tablica LUT
            for (int i = 0; i < pixelValues.Length; i++)
            {
                pixelValues[i] = LUT[pixelValues[i]];
            }
            //Ustaw wartosc wszystkich punktow obrazu
            Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            bitmap.UnlockBits(bmpData);
            return bitmap;

            #endregion
        }

        private static Bitmap Rozciaganie2(Bitmap sourceBitmap, int threshold1, int threshold2)
        {
            #region Rozciaganie
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float blue = 0;
            float green = 0;
            float red = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                if (threshold1 <= threshold2)
                {
                    if (pixelBuffer[k] < threshold1)
                    {
                        blue = 0;
                    }
                    if (pixelBuffer[k + 1] < threshold1)
                    {
                        green = 0;
                    }
                    if (pixelBuffer[k + 2] < threshold1)
                    {
                        red = 0;
                    }
                    if (pixelBuffer[k] > threshold2)
                    {
                        blue = 255;
                    }
                    if (pixelBuffer[k + 1] > threshold2)
                    {
                        green = 255;
                    }
                    if (pixelBuffer[k + 2] > threshold2)
                    {
                        red = 255;
                    }
                    else if (threshold1 != threshold2)
                    {
                        blue = 255 * (pixelBuffer[k] - threshold1) / ((float)(threshold2 - threshold1));
                        green = 255 * (pixelBuffer[k + 1] - threshold1) / ((float)(threshold2 - threshold1));
                        red = 255 * (pixelBuffer[k + 2] - threshold1) / ((float)(threshold2 - threshold1));
                    }
                }

                if (threshold2 < threshold1)
                {
                    if (pixelBuffer[k] < threshold2)
                    {
                        blue = 0;
                    }
                    if (pixelBuffer[k + 1] < threshold2)
                    {
                        green = 0;
                    }
                    if (pixelBuffer[k + 2] < threshold2)
                    {
                        red = 0;
                    }
                    if (pixelBuffer[k] > threshold1)
                    {
                        blue = 255;
                    }
                    if (pixelBuffer[k + 1] > threshold1)
                    {
                        green = 255;
                    }
                    if (pixelBuffer[k + 2] > threshold1)
                    {
                        red = 255;
                    }
                    else
                    {
                        blue = 255 * (pixelBuffer[k] - threshold2) / ((float)(threshold1 - threshold2));
                        green = 255 * (pixelBuffer[k + 1] - threshold2) / ((float)(threshold1 - threshold2));
                        red = 255 * (pixelBuffer[k + 2] - threshold2) / ((float)(threshold1 - threshold2));
                    }

                }


                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }

                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }

                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;

            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
            #endregion
        }

        public static Bitmap RozciaganieRGB(Bitmap sourceBitmap, int threshold1, int threshold2)
        {
            #region Tabkica LUT - ROZCIĄGANIE RGB
            //Przygotuj tablice LUT i pokaz ja na wykresie
            // wykres.Series[0].Points.Clear();
            byte[] LUT = new byte[256];
            //   int b = this.trackBarOPJ.Value;
            //  int b = this.trackBar1.Value;
            // wykres.Titles[0].Text = "Tablica LUT, p1 = " + a + " p2 = " + b;

            for (int i = 0; i < 256; ++i)
            {
                if ((threshold1 - threshold2) <= 0 || (threshold2 - threshold1) <= 0)
                {
                    LUT[i] = 255;
                }
                if (threshold1 > threshold2)        // a - high, b low
                {
                    float p = 255.0f / (threshold1 - threshold2);
                    if (i < threshold2)
                        LUT[i] = 0;
                    else if (i >= threshold1)
                        LUT[i] = 0;
                    else
                        LUT[i] = (byte)((i - threshold2) * p);
                }

                if (threshold1 < threshold2)        // b - high, a low
                {
                    float p = 255.0f / (threshold2 - threshold1);
                    if (i < threshold1)
                        LUT[i] = 0;
                    else if (i >= threshold2)
                        LUT[i] = 0;
                    else
                        LUT[i] = (byte)((i - threshold1) * p);
                }
                //wykres.Series[0].Points.Add(new DataPoint(i, LUT[i]));
            }

            return Rozciaganie2(sourceBitmap, threshold1, threshold2);

            #endregion
        }

        public static Bitmap RozciagnieHSV2(Bitmap sourceBitmap, int threshold, int threshold2)
        {
            #region RozciagnieHSV2
            Bitmap newBmp = (Bitmap)sourceBitmap.Clone();
            FastBitmap fobrazek_bitmapa = new FastBitmap(newBmp);
            fobrazek_bitmapa.LockImage();

            int hI = sourceBitmap.Height;
            int wI = sourceBitmap.Width;

            for (int x = 0; x < wI; x++)
            {
                for (int y = 0; y < hI; y++)
                {
                    double h, s, v;
                    hsv.ColorToHSV(fobrazek_bitmapa.GetPixel(x, y), out h, out s, out v);
                    double wartosc = v;
                    if (wartosc < ((double)threshold) / 255)
                    {
                        fobrazek_bitmapa.SetPixel(x, y, hsv.ColorFromHSV(h, s, 0));
                    }
                    else if (wartosc > ((double)threshold2) / 255)
                    {
                        fobrazek_bitmapa.SetPixel(x, y, hsv.ColorFromHSV(h, s, 1));
                    }
                    else
                    {
                        double tmp = wartosc;
                        if (threshold2 != threshold)
                        {
                            tmp = (((wartosc - ((double)threshold / 255.0f)))) / ((threshold2 - threshold) / 255.0f);
                        }

                        fobrazek_bitmapa.SetPixel(x, y, hsv.ColorFromHSV(h, s, tmp));
                    }
                }
            }
            fobrazek_bitmapa.UnlockImage();
            return newBmp;
            #endregion
        }



        public static Bitmap AddMethod(Bitmap bmp1, Bitmap bmp2, bool isHsv = false)
        {
            Bitmap newBmp = (Bitmap)bmp1.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();
            FastBitmap lockBitmap2 = new FastBitmap(bmp2);
            lockBitmap2.LockImage();

            double[] hvalue = new double[3];
            double[] svalue = new double[3];
            double[] vvalue = new double[3];
            for (int x = 0; x < newBmp.Width; x++)
            {
                for (int y = 0; y < newBmp.Height; y++ )
                {
                    Color color1 = lockBitmap.GetPixel(x, y);
                    Color color2;
                    if (bmp2.Height > y && bmp2.Width > x)
                    {
                        color2 = lockBitmap2.GetPixel(x, y);
                    }
                    else
                    {
                        break;
                    }

                    var rvalue = 0;
                    var gvalue = 0;
                    var bvalue = 0;
                    if (isHsv)
                    {
                        hsv.ColorToHSV(color1, out hvalue[1], out svalue[1], out vvalue[1]);
                        hsv.ColorToHSV(color2, out hvalue[2], out svalue[2], out vvalue[2]);
                        hvalue[0] = hvalue[1] + hvalue[2] > 360
                        ? 360
                        : hvalue[1] + hvalue[2];

                        svalue[0] = svalue[1] + svalue[2] > 1
                        ? 1
                        : svalue[1] + svalue[2];

                        vvalue[0] = vvalue[1] + vvalue[2] > 1
                        ? 1
                        : vvalue[1] + vvalue[2];

                        Color color = hsv.ColorFromHSV(hvalue[0], svalue[0], vvalue[0]);
                        rvalue = color.R;
                        gvalue = color.G;
                        bvalue = color.B;
                    }
                    else
                    {
                        rvalue = color1.R + color2.R;
                        if (rvalue > 255)
                            rvalue = 255;
                        gvalue = color1.G + color2.G;
                        if (gvalue > 255)
                            gvalue = 255;
                        bvalue = color1.B + color2.B;
                        if (bvalue > 255)
                            bvalue = 255;

                    }
                    lockBitmap.SetPixel(x, y, Color.FromArgb(rvalue, gvalue, bvalue));

                }
            }

            lockBitmap2.UnlockImage();
            lockBitmap.UnlockImage();
            return newBmp;
        }

        public static Bitmap SubMethod(Bitmap bmp1, Bitmap bmp2, bool isHsv = false)
        {
            Bitmap newBmp = (Bitmap)bmp1.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();
            FastBitmap lockBitmap2 = new FastBitmap(bmp2);
            lockBitmap2.LockImage();

            double[] hvalue = new double[3];
            double[] svalue = new double[3];
            double[] vvalue = new double[3];
            for (int j = 0; j < bmp1.Height; j++)
            {
                for (int i = 0; i < bmp1.Width; i++)
                {
                    var color1 = lockBitmap.GetPixel(i, j);
                    Color color2;
                    if (bmp2.Height > j && bmp2.Width > i)
                    {
                        color2 = lockBitmap2.GetPixel(i, j);
                    }
                    else
                    {
                        break;
                    }

                    var rvalue = 0;
                    var gvalue = 0;
                    var bvalue = 0;
                    if (isHsv)
                    {
                        hsv.ColorToHSV(color1, out hvalue[1], out svalue[1], out vvalue[1]);
                        hsv.ColorToHSV(color2, out hvalue[2], out svalue[2], out vvalue[2]);
                        hvalue[0] = hvalue[1] - hvalue[2] < 0
                        ? 0
                        : hvalue[1] - hvalue[2];

                        svalue[0] = svalue[1] - svalue[2] < 0
                        ? 0
                        : svalue[1] - svalue[2];

                        vvalue[0] = vvalue[1] - vvalue[2] < 0
                        ? 0
                        : vvalue[1] - vvalue[2];

                        Color color = hsv.ColorFromHSV(hvalue[0], svalue[0], vvalue[0]);
                        rvalue = color.R;
                        gvalue = color.G;
                        bvalue = color.B;
                    }
                    else
                    {
                        rvalue = color1.R - color2.R;
                        rvalue = rvalue < 0 ? 0 : rvalue;

                        gvalue = color1.G - color2.G;
                        gvalue = gvalue < 0 ? 0 : gvalue;

                        bvalue = color1.B - color2.B;
                        bvalue = bvalue < 0 ? 0 : bvalue;
                    }
                    lockBitmap.SetPixel(i, j, Color.FromArgb(rvalue, gvalue, bvalue));


                }
            }

            lockBitmap2.UnlockImage();
            lockBitmap.UnlockImage();
            return newBmp;
        }

        public static Bitmap DifferenceMethod(Bitmap bmp1, Bitmap bmp2, bool isHsv = false)
        {
            Bitmap newBmp = (Bitmap)bmp1.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();
            FastBitmap lockBitmap2 = new FastBitmap(bmp2);
            lockBitmap2.LockImage();

            double[] hvalue = new double[3];
            double[] svalue = new double[3];
            double[] vvalue = new double[3];
            for (int y = 0; y < bmp1.Height; y++)
            {
                for (int x = 0; x < bmp1.Width; x++)
                {
                    var color1 = lockBitmap.GetPixel(x, y);
                    Color color2;
                    if (bmp2.Height > y && bmp2.Width > x)
                    {
                        color2 = lockBitmap2.GetPixel(x, y);
                    }
                    else
                    {
                        break;
                    }

                    var rvalue = 0;
                    var gvalue = 0;
                    var bvalue = 0;
                    if (isHsv)
                    {
                        hsv.ColorToHSV(color1, out hvalue[1], out svalue[1], out vvalue[1]);
                        hsv.ColorToHSV(color2, out hvalue[2], out svalue[2], out vvalue[2]);
                        hvalue[0] = hvalue[1] - hvalue[2] < 0
                        ? 0
                        : Math.Abs(hvalue[1] - hvalue[2]);

                        svalue[0] = svalue[1] - svalue[2] < 0
                        ? 0
                        : Math.Abs(svalue[1] - svalue[2]);

                        vvalue[0] = vvalue[1] - vvalue[2] < 0
                        ? 0
                        : Math.Abs(vvalue[1] - vvalue[2]);

                        Color color = hsv.ColorFromHSV(hvalue[0], svalue[0], vvalue[0]);
                        rvalue = color.R;
                        gvalue = color.G;
                        bvalue = color.B;
                    }
                    else
                    {
                        rvalue = Math.Abs(color1.R - color2.R) < 0
                        ? 0
                        : Math.Abs(color1.R - color2.R);

                        gvalue = Math.Abs(color1.G - color2.G) < 0
                            ? 0
                            : Math.Abs(color1.G - color2.G);

                        bvalue = Math.Abs(color1.B - color2.B) < 0
                            ? 0
                            : Math.Abs(color1.B - color2.B);
                    }
                    lockBitmap.SetPixel(x, y, Color.FromArgb(rvalue, gvalue, bvalue));


                }
            }

            lockBitmap2.UnlockImage();
            lockBitmap.UnlockImage();
            return newBmp;
        }

        public static Bitmap AndMethod(Bitmap bmp1, Bitmap bmp2, bool isHsv = false)
        {
            Bitmap newBmp = (Bitmap)bmp1.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();
            FastBitmap lockBitmap2 = new FastBitmap(bmp2);
            lockBitmap2.LockImage();

            double[] hvalue = new double[3];
            double[] svalue = new double[3];
            double[] vvalue = new double[3];
            for (int j = 0; j < bmp1.Height; j++)
            {
                for (int i = 0; i < bmp1.Width; i++)
                {

                    var color1 = lockBitmap.GetPixel(i, j);
                    Color color2;
                    if (bmp2.Height > j && bmp2.Width > i)
                    {
                        color2 = lockBitmap2.GetPixel(i, j);
                    }
                    else
                    {
                        break;
                    }

                    var rvalue = 0;
                    var gvalue = 0;
                    var bvalue = 0;
                    if (isHsv)
                    {
                        hsv.ColorToHSV(color1, out hvalue[1], out svalue[1], out vvalue[1]);
                        hsv.ColorToHSV(color2, out hvalue[2], out svalue[2], out vvalue[2]);
                        //hvalue[0] = hvalue[1] & hvalue[2];
                        //svalue[0] = svalue[1] & svalue[2];
                        //vvalue[0] = vvalue[1] & vvalue[2];

                        Color color = hsv.ColorFromHSV(hvalue[0], svalue[0], vvalue[0]);
                        rvalue = color.R;
                        gvalue = color.G;
                        bvalue = color.B;
                    }
                    else
                    {

                        rvalue = color1.R & color2.R;

                        gvalue = color1.G & color2.G;

                        bvalue = color1.B & color2.B;
                    }
                    lockBitmap.SetPixel(i, j, Color.FromArgb(rvalue, gvalue, bvalue));


                }
            }

            lockBitmap2.UnlockImage();
            lockBitmap.UnlockImage();
            return newBmp;
        }

        public static Bitmap OrMethod(Bitmap bmp1, Bitmap bmp2, bool isHsv = false)
        {
            Bitmap newBmp = (Bitmap)bmp1.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();
            FastBitmap lockBitmap2 = new FastBitmap(bmp2);
            lockBitmap2.LockImage();

            int rvalue = 0;
            int gvalue = 0;
            int bvalue = 0;
            double[] hvalue = new double[3];
            double[] svalue = new double[3];
            double[] vvalue = new double[3];
            for (int j = 0; j < bmp1.Height; j++)
            {
                for (int i = 0; i < bmp1.Width; i++)
                {
                    var color1 = lockBitmap.GetPixel(i, j);
                    Color color2;
                    if (bmp2.Height > j && bmp2.Width > i)
                    {
                        color2 = lockBitmap2.GetPixel(i, j);
                    }
                    else
                    {
                        break;
                    }
                    if (isHsv)
                    {
                        hsv.ColorToHSV(color1, out hvalue[1], out svalue[1], out vvalue[1]);
                        hsv.ColorToHSV(color2, out hvalue[2], out svalue[2], out vvalue[2]);
                        hvalue[0] = hvalue[1] + hvalue[2] > 360
                        ? 360
                        : hvalue[1] + hvalue[2];

                        svalue[0] = svalue[1] + svalue[2] > 1
                        ? 1
                        : svalue[1] + svalue[2];

                        vvalue[0] = vvalue[1] + vvalue[2] > 1
                        ? 1
                        : vvalue[1] + vvalue[2];

                        Color color = hsv.ColorFromHSV(hvalue[0], svalue[0], vvalue[0]);
                        rvalue = color.R;
                        gvalue = color.G;
                        bvalue = color.B;
                    }
                    else
                    {
                        rvalue = color1.R | color2.R;

                        gvalue = color1.G | color2.G;
                        bvalue = color1.B | color2.B;
                    }
                    lockBitmap.SetPixel(i, j, Color.FromArgb(rvalue, gvalue, bvalue));

                }
            }

            lockBitmap2.UnlockImage();
            lockBitmap.UnlockImage();
            return newBmp;
        }

        public static Bitmap XorMethod(Bitmap bmp1, Bitmap bmp2, bool isHsv = false)
        {
            Bitmap newBmp = (Bitmap)bmp1.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();
            FastBitmap lockBitmap2 = new FastBitmap(bmp2);
            lockBitmap2.LockImage();

            double[] hvalue = new double[3];
            double[] svalue = new double[3];
            double[] vvalue = new double[3];
            for (int j = 0; j < bmp1.Height; j++)
            {
                for (int i = 0; i < bmp1.Width; i++)
                {
                    var color1 = lockBitmap.GetPixel(i, j);
                    Color color2;
                    if (bmp2.Height > j && bmp2.Width > i)
                    {
                        color2 = lockBitmap2.GetPixel(i, j);
                    }
                    else
                    {
                        break;
                    }

                    var rvalue = 0;
                    var gvalue = 0;
                    var bvalue = 0;
                    if (isHsv)
                    {
                        hsv.ColorToHSV(color1, out hvalue[1], out svalue[1], out vvalue[1]);
                        hsv.ColorToHSV(color2, out hvalue[2], out svalue[2], out vvalue[2]);
                        hvalue[0] = hvalue[1] + hvalue[2] > 360
                        ? 360
                        : hvalue[1] + hvalue[2];

                        svalue[0] = svalue[1] + svalue[2] > 1
                        ? 1
                        : svalue[1] + svalue[2];

                        vvalue[0] = vvalue[1] + vvalue[2] > 1
                        ? 1
                        : vvalue[1] + vvalue[2];

                        Color color = hsv.ColorFromHSV(hvalue[0], svalue[0], vvalue[0]);
                        rvalue = color.R;
                        gvalue = color.G;
                        bvalue = color.B;
                    }
                    else
                    {
                        rvalue = color1.R ^ color2.R;

                        gvalue = color1.G ^ color2.G;

                        bvalue = color1.B ^ color2.B;
                    }
                    lockBitmap.SetPixel(i, j, Color.FromArgb(rvalue, gvalue, bvalue));


                }
            }

            lockBitmap2.UnlockImage();
            lockBitmap.UnlockImage();
            return newBmp;
        }

    }
}
