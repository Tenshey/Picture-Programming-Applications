using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using APO_PJ_AP.Model;

namespace APO_PJ_AP.Operation
{
    class HistogramOperation
    {
        public enum MetodyWyrownywania
        {
            Srednich,
            Losowych,
            Sasiedztwa,
            Wlasna
        }

        public static Bitmap histogram_wyrownanie(Bitmap bmp, MetodyWyrownywania metoda)
        {
            #region histogram_wyrownanie
            int[,] histogram_values = new int[3, 256];
            int[] histogram_max = { 0, 0, 0 };
            int[] histogram_avg = { 0, 0, 0 };
            Bitmap obrazekBitmapa = (Bitmap)bmp.Clone();
            FastBitmap fobrazek_bitmapa = new FastBitmap(obrazekBitmapa);
            fobrazek_bitmapa.LockImage();
            //Tworzenie tabeli histogramu
            for (int x = 0; x < obrazekBitmapa.Width; x++)
            {
                for (int y = 0; y < obrazekBitmapa.Height; y++)
                {
                    histogram_values[0, fobrazek_bitmapa.GetPixel(x, y).R]++;  // wypełnienie tabeli histogramu wartościami z kanału czerwonego
                    histogram_values[1, fobrazek_bitmapa.GetPixel(x, y).G]++;
                    histogram_values[2, fobrazek_bitmapa.GetPixel(x, y).B]++;
                    if (histogram_values[0, fobrazek_bitmapa.GetPixel(x, y).R] > histogram_max[0])
                        histogram_max[0] = histogram_values[0, fobrazek_bitmapa.GetPixel(x, y).R];    // najwyższa wartość w tabeli histogramu
                    if (histogram_values[1, fobrazek_bitmapa.GetPixel(x, y).G] > histogram_max[0])
                        histogram_max[1] = histogram_values[1, fobrazek_bitmapa.GetPixel(x, y).G];
                    if (histogram_values[2, fobrazek_bitmapa.GetPixel(x, y).B] > histogram_max[0])
                        histogram_max[2] = histogram_values[2, fobrazek_bitmapa.GetPixel(x, y).B];
                }
            }
            fobrazek_bitmapa.UnlockImage();
            //Wyliczanie sredniej
            long[] tmp = { 0, 0, 0 };
            for (int i = 0; i < histogram_values.Length / 3; i++)
            {
                tmp[0] += histogram_values[0, i];
                tmp[1] += histogram_values[1, i];
                tmp[2] += histogram_values[2, i];
            }
            histogram_avg[0] = (int)(tmp[0] / 256);   // suma wszystkich elementów podzielona przez 256
            histogram_avg[1] = (int)(tmp[1] / 256);
            histogram_avg[2] = (int)(tmp[2] / 256);
            int[,] left = new int[3, 256];
            int[,] right = new int[3, 256];
            int[,] nowy = new int[3, 256];
            int r = 0, g = 0, b = 0;
            int[] h_int = { 0, 0, 0 };
            for (int z = 0; z < 256; z++)
            {
                left[0, z] = r;
                if (left[0, z] == 256)
                    left[0, z]--;
                h_int[0] += histogram_values[0, z];
                while (h_int[0] > histogram_avg[0])
                {
                    h_int[0] -= histogram_avg[0];
                    r++;
                }
                right[0, z] = r;
                if (right[0, z] == 256)
                    right[0, z]--;

                left[1, z] = g;
                if (left[1, z] == 256)
                    left[1, z]--;
                h_int[1] += histogram_values[1, z];
                while (h_int[1] > histogram_avg[1])
                {
                    h_int[1] -= histogram_avg[1];
                    g++;
                }
                right[1, z] = g;
                if (right[1, z] == 256)
                    right[1, z]--;

                left[2, z] = b;
                if (left[2, z] == 256)
                    left[2, z]--;
                h_int[2] += histogram_values[2, z];
                while (h_int[2] > histogram_avg[2])
                {
                    h_int[2] -= histogram_avg[2];
                    b++;
                }
                right[2, z] = b;
                if (right[2, z] == 256)
                    right[2, z]--;

                if (metoda == MetodyWyrownywania.Srednich)
                {
                    //metoda1 - metoda średnich
                    nowy[0, z] = (left[0, z] + right[0, z]) / 2;
                    nowy[1, z] = (left[1, z] + right[1, z]) / 2;
                    nowy[2, z] = (left[2, z] + right[2, z]) / 2;
                }
                else if (metoda == MetodyWyrownywania.Losowych)
                {
                    //metoda2 - metoda losowych
                    if (right[0, z] >= left[0, z])
                        nowy[0, z] = (right[0, z] - left[0, z]);
                    if (right[1, z] >= left[1, z])
                        nowy[1, z] = (right[1, z] - left[1, z]);
                    if (right[2, z] >= left[2, z])
                        nowy[2, z] = (right[2, z] - left[2, z]);
                }
                else if (metoda == MetodyWyrownywania.Sasiedztwa)
                {
                    //metoda3 - sąsiedztwa                 
                    if (right[0, z] >= left[0, z])
                        nowy[0, z] = (right[0, z] - left[0, z]) / 2;
                    if (right[1, z] >= left[1, z])
                        nowy[1, z] = (right[1, z] - left[1, z]) / 2;
                    if (right[2, z] >= left[2, z])
                        nowy[2, z] = (right[2, z] - left[2, z]) / 2;
                }
                else if (metoda == MetodyWyrownywania.Wlasna)
                {
                    //metoda4 - własna
                    if (right[0, z] >= left[0, z])
                        nowy[0, z] = left[0, z];
                    if (right[1, z] >= left[1, z])
                        nowy[1, z] = left[1, z];
                    if (right[2, z] >= left[2, z])
                        nowy[2, z] = left[2, z];
                }
            }
            fobrazek_bitmapa.LockImage();
            for (int x = 0; x < obrazekBitmapa.Width; x++)
            {
                for (int y = 0; y < obrazekBitmapa.Height; y++)
                {
                    if (left[0, fobrazek_bitmapa.GetPixel(x, y).R] == right[0, fobrazek_bitmapa.GetPixel(x, y).R])
                    {
                        fobrazek_bitmapa.SetPixel(x, y, Color.FromArgb(255, left[0, fobrazek_bitmapa.GetPixel(x, y).R], left[1, fobrazek_bitmapa.GetPixel(x, y).G], left[2, fobrazek_bitmapa.GetPixel(x, y).B]));
                    }
                    else
                    {
                        if (metoda == MetodyWyrownywania.Srednich)
                        {
                            fobrazek_bitmapa.SetPixel(x, y, Color.FromArgb(255, nowy[0, fobrazek_bitmapa.GetPixel(x, y).R], nowy[1, fobrazek_bitmapa.GetPixel(x, y).G], nowy[2, fobrazek_bitmapa.GetPixel(x, y).B]));
                        }
                        else if (metoda == MetodyWyrownywania.Losowych)
                        {
                            Random rnd = new Random();
                            int[] rndnum = { 0, 0, 0 };
                            rndnum[0] = rnd.Next(0, nowy[0, fobrazek_bitmapa.GetPixel(x, y).R]) + left[0, fobrazek_bitmapa.GetPixel(x, y).R];
                            if (rndnum[0] > 255)
                                rndnum[0] = 255;
                            rndnum[1] = rnd.Next(0, nowy[1, fobrazek_bitmapa.GetPixel(x, y).G]) + left[1, fobrazek_bitmapa.GetPixel(x, y).G];
                            if (rndnum[1] > 255)
                                rndnum[1] = 255;
                            rndnum[2] = rnd.Next(0, nowy[1, fobrazek_bitmapa.GetPixel(x, y).B]) + left[1, fobrazek_bitmapa.GetPixel(x, y).B];
                            if (rndnum[2] > 255)
                                rndnum[2] = 255;
                            fobrazek_bitmapa.SetPixel(x, y, Color.FromArgb(255, rndnum[0], rndnum[1], rndnum[2]));
                        }
                        else if (metoda == MetodyWyrownywania.Sasiedztwa)
                        {
                            int[] piks_avg = { 0, 0, 0 };
                            if (x > 0)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x - 1, y).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x - 1, y).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x - 1, y).B;
                            }
                            if (y > 0)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x, y - 1).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x, y - 1).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x, y - 1).B;
                            }
                            if (x < obrazekBitmapa.Width - 1)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x + 1, y).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x + 1, y).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x + 1, y).B;
                            }
                            if (y < obrazekBitmapa.Height - 1)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x, y + 1).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x, y + 1).R;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x, y + 1).R;
                            }
                            piks_avg[0] /= 4;
                            piks_avg[1] /= 4;
                            piks_avg[2] /= 4;
                            if (piks_avg[0] < left[0, fobrazek_bitmapa.GetPixel(x, y).R])
                                piks_avg[0] = left[0, fobrazek_bitmapa.GetPixel(x, y).R];
                            else if (piks_avg[0] > right[0, fobrazek_bitmapa.GetPixel(x, y).R])
                                piks_avg[0] = right[0, fobrazek_bitmapa.GetPixel(x, y).R];
                            if (piks_avg[1] < left[1, fobrazek_bitmapa.GetPixel(x, y).G])
                                piks_avg[1] = left[1, fobrazek_bitmapa.GetPixel(x, y).G];
                            else if (piks_avg[1] > right[1, fobrazek_bitmapa.GetPixel(x, y).G])
                                piks_avg[1] = right[1, fobrazek_bitmapa.GetPixel(x, y).G];
                            if (piks_avg[2] < left[2, fobrazek_bitmapa.GetPixel(x, y).B])
                                piks_avg[2] = left[2, fobrazek_bitmapa.GetPixel(x, y).B];
                            else if (piks_avg[2] > right[2, fobrazek_bitmapa.GetPixel(x, y).B])
                                piks_avg[2] = right[2, fobrazek_bitmapa.GetPixel(x, y).B];
                            fobrazek_bitmapa.SetPixel(x, y, Color.FromArgb(255, piks_avg[0], piks_avg[1], piks_avg[2]));
                        }
                        else if (metoda == MetodyWyrownywania.Wlasna)
                        {
                            int[] piks_avg = { 0, 0, 0 };
                            if (x > 0)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x - 1, y).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x - 1, y).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x - 1, y).B;
                            }
                            if (y > 0)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x, y - 1).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x, y - 1).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x, y - 1).B;
                            }
                            if (x < obrazekBitmapa.Width - 1)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x + 1, y).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x + 1, y).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x + 1, y).B;
                            }
                            if (y < obrazekBitmapa.Height - 1)
                            {
                                piks_avg[0] = +fobrazek_bitmapa.GetPixel(x, y + 1).R;
                                piks_avg[1] = +fobrazek_bitmapa.GetPixel(x, y + 1).G;
                                piks_avg[2] = +fobrazek_bitmapa.GetPixel(x, y + 1).B;
                            }
                            piks_avg[0] /= 4;
                            piks_avg[1] /= 4;
                            piks_avg[2] /= 4;
                            if (piks_avg[0] < left[0, fobrazek_bitmapa.GetPixel(x, y).R])
                                piks_avg[0] = left[0, fobrazek_bitmapa.GetPixel(x, y).R];
                            else if (piks_avg[0] > right[0, fobrazek_bitmapa.GetPixel(x, y).R])
                                piks_avg[0] = right[0, fobrazek_bitmapa.GetPixel(x, y).R];
                            if (piks_avg[1] < left[1, fobrazek_bitmapa.GetPixel(x, y).G])
                                piks_avg[1] = left[1, fobrazek_bitmapa.GetPixel(x, y).G];
                            else if (piks_avg[1] > right[1, fobrazek_bitmapa.GetPixel(x, y).G])
                                piks_avg[1] = right[1, fobrazek_bitmapa.GetPixel(x, y).G];
                            if (piks_avg[2] < left[2, fobrazek_bitmapa.GetPixel(x, y).B])
                                piks_avg[2] = left[2, fobrazek_bitmapa.GetPixel(x, y).B];
                            else if (piks_avg[2] > right[2, fobrazek_bitmapa.GetPixel(x, y).B])
                                piks_avg[2] = right[2, fobrazek_bitmapa.GetPixel(x, y).B];
                            fobrazek_bitmapa.SetPixel(x, y, Color.FromArgb(255, piks_avg[0], piks_avg[1], piks_avg[2]));
                        }
                    }
                }
            }
            fobrazek_bitmapa.UnlockImage();
            return obrazekBitmapa;
            #endregion
        }

        public static Bitmap NeighborhoodMethod(ImageModel img)
        { 
            Bitmap newBmp = (Bitmap)img.Bmp.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();

            int rvalue = 0, gvalue = 0, bvalue = 0;
            for (int i = 0; i < newBmp.Width; i++)
                for (int j = 0; j < newBmp.Height; j++)
                {
                    foreach (Point offset in new[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1), new Point(1, 1), new Point(-1, -1), new Point(-1, 1), new Point(1, -1) })
                    {
                        if (i + offset.X >= 0 && i + offset.X < newBmp.Width && j + offset.Y >= 0 && j + offset.Y < newBmp.Height)
                        {
                            Color color = lockBitmap.GetPixel(i + offset.X, j + offset.Y);
                            rvalue += Convert.ToInt32(color.R);
                            gvalue += Convert.ToInt32(color.G);
                            bvalue += Convert.ToInt32(color.B);
                        }
                    }
                    lockBitmap.SetPixel(i, j, Color.FromArgb(rvalue/9, gvalue/9, bvalue/9));
                    rvalue = 0;
                    gvalue = 0;
                    bvalue = 0;
                }

            lockBitmap.UnlockImage();
            return newBmp;
        }

        public static Bitmap OwnMethod(Bitmap bmp)
        {
            Bitmap newBmp = (Bitmap)bmp.Clone();
            FastBitmap lockBitmap = new FastBitmap(newBmp);
            lockBitmap.LockImage();

            int[,] histogram_values = new int[3, 256];
            for (int x = 0; x < newBmp.Width; x++)
            {
                for (int y = 0; y < newBmp.Height; y++)
                {
                    histogram_values[0, lockBitmap.GetPixel(x, y).R]++;
                    histogram_values[1, lockBitmap.GetPixel(x, y).G]++;
                    histogram_values[2, lockBitmap.GetPixel(x, y).B]++;
                }
            }

            int i = 0;
            double[,] cumulativeDistributionTab = new double[3, 256];
            double[] sumValues = new double[3];

            int numberOfPixels = newBmp.Width * newBmp.Height;

            for (i = 0; i < 256; i++)
            {
                sumValues[0] += (double)histogram_values[0, i] / numberOfPixels;
                cumulativeDistributionTab[0, i] = sumValues[0];

                sumValues[1] += (double)histogram_values[1, i] / numberOfPixels;
                sumValues[2] += (double)histogram_values[2, i] / numberOfPixels;

                cumulativeDistributionTab[1, i] = sumValues[1];
                cumulativeDistributionTab[2, i] = sumValues[2];
            }

            double[,] LUT = new double[3, 256];
            double[] D0min = new double[3];

            i = 0;
            while (cumulativeDistributionTab[0, i].Equals(0)) i++;
            D0min[0] = cumulativeDistributionTab[0, i];

            i = 0;
            while (cumulativeDistributionTab[1, i].Equals(0)) i++;
            D0min[1] = cumulativeDistributionTab[1, i];

            i = 0;
            while (cumulativeDistributionTab[2, i].Equals(0)) i++;
            D0min[2] = cumulativeDistributionTab[2, i];

            for (i = 0; i < 256; i++)
            {
                LUT[0,i] = (cumulativeDistributionTab[0,i] - D0min[0]) / (1 - D0min[0]) * (256 - 1);
                LUT[1, i] = (cumulativeDistributionTab[1, i] - D0min[1]) / (1 - D0min[1]) * (256 - 1);
                LUT[2, i] = (cumulativeDistributionTab[2, i] - D0min[2]) / (1 - D0min[2]) * (256 - 1);
            }


            for (i = 0; i < newBmp.Width; i++)
            {
                for (int j = 0; j < newBmp.Height; j++)
                {
                    Color color = lockBitmap.GetPixel(i, j);

                    int rvalue = Convert.ToInt32(color.R);
                    int gvalue = Convert.ToInt32(color.G);
                    int bvalue = Convert.ToInt32(color.B);
                    
                    color = Color.FromArgb((int)LUT[0, rvalue], (int)LUT[1, gvalue], (int)LUT[2, bvalue]);

                    lockBitmap.SetPixel(i, j, color);
                }
            }

            lockBitmap.UnlockImage();
            return newBmp;
        }

        #region Functions
        public static Bitmap ConvertToGrayscale(Bitmap bmp)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            int kolorr = 0;
            int kolorg = 0;
            int kolorb = 0;
            int nowy = 0;
            Color nowyKolor = new Color();
            for (int i = 0; i < newBmp.Width; i++)
            {
                for (int j = 0; j < newBmp.Height; j++)
                {
                    kolorr = bmp.GetPixel(i, j).R;
                    kolorg = bmp.GetPixel(i, j).G;
                    kolorb = bmp.GetPixel(i, j).B;

                    if ((kolorr > kolorg && kolorr < kolorb) || (kolorr < kolorg) && (kolorr > kolorb))
                    {
                        nowy = kolorr;
                    }
                    else if ((kolorg > kolorr && kolorg < kolorb) || (kolorg < kolorr) && (kolorg > kolorb))
                    {
                        nowy = kolorg;
                    }
                    else
                    {
                        nowy = kolorb;
                    }

                    nowyKolor = Color.FromArgb(nowy, nowy, nowy);
                    newBmp.SetPixel(i, j, nowyKolor);
                }
            }
            return newBmp;
        }

        private static void updateLUT(double[] D, int[] LUT)
        {
            int i;
            double D0min;

            i = 0;
            while (D[i] == 0) i++;
            D0min = D[i];

            for (i = 0; i < 256; i++)
            {
                LUT[i] = (int)(((D[i] - D0min) / (1 - D0min)) * (256 - 1));
            }
        }

        private static void updateLUT2(double a, int b, int[] LUT)
        {
            for (int i = 0; i < 256; i++)
                if ((a * (i + b)) > 255)
                    LUT[i] = 255;
                else if ((a * (i + b)) < 0)
                    LUT[i] = 0;
                else
                    LUT[i] = (int)(a * (i + b));
        }

        #endregion

        #region HistogramStretchingCommand

        public static Bitmap histogram_Stretching(Bitmap bmp_source)
        {
            Bitmap bmp = (Bitmap)bmp_source.Clone();

            int[] kolorRgb = new int[3];

            int rmin = 255;

            int rmax = 1;

            int[] LUTr = new int[256];
            int[] LUTg = new int[256];
            int[] LUTb = new int[256];
            Color color;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    color = bmp.GetPixel(i, j);
                    kolorRgb[0] = Convert.ToInt32(color.R);
                    kolorRgb[1] = Convert.ToInt32(color.G);
                    kolorRgb[2] = Convert.ToInt32(color.B);

                    if (kolorRgb[0] > rmax) rmax = kolorRgb[0];

                    if (kolorRgb[0] < rmin) rmin = kolorRgb[0];

                    if (kolorRgb[1] > rmax) rmax = kolorRgb[1];

                    if (kolorRgb[1] < rmin) rmin = kolorRgb[1];

                    if (kolorRgb[2] > rmax) rmax = kolorRgb[2];

                    if (kolorRgb[2] < rmin) rmin = kolorRgb[2];
                }
            }

            updateLUT2(255.0 / (rmax - rmin), -rmin, LUTr);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    color = bmp.GetPixel(i, j);
                    kolorRgb[0] = Convert.ToInt32(color.R);
                    kolorRgb[1] = Convert.ToInt32(color.G);
                    kolorRgb[2] = Convert.ToInt32(color.B);

                    color = Color.FromArgb(LUTr[kolorRgb[0]], LUTr[kolorRgb[1]], LUTr[kolorRgb[2]]);

                    if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                    {
                        BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                        byte[] bytes = new byte[data.Height * data.Stride];
                        Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
                        bytes[j * data.Stride + i] = color.R;
                        Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
                        bmp.UnlockBits(data);
                    }
                    else
                    {
                        bmp.SetPixel(i, j, color);
                    }
                }
            }
            return bmp;
        }
        #endregion
    }
}
