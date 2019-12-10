using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace APO_PJ_AP.Operation
{
    public static class ImageFilter
    {
        public enum Spojnosc
        {
            Czterospojne,
            Osmiospojne
        }

        public enum MorphologyType
        {
            Erosion,
            Dilatation
        }

        public static Bitmap MedianFilter(this Bitmap sourceBitmap, int matrixSize)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(pixelBuffer, calcOffset));
                        }
                    }
                    neighbourPixels.Sort();
                    middlePixel = BitConverter.GetBytes(neighbourPixels[filterOffset]);
                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }


        public static Bitmap DilateAndErodeFilter(Bitmap sourceBitmap, int matrixSize, MorphologyType morphType, Spojnosc spojnoscType, bool applyBlue = true, bool applyGreen = true, bool applyRed = true)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);
            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            byte blue = 0;
            byte green = 0;
            byte red = 0;
            byte morphResetValue = 0;

            if (morphType == MorphologyType.Erosion)
            {
                morphResetValue = 255;
            }

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;

                    blue = morphResetValue;
                    green = morphResetValue;
                    red = morphResetValue;

                    if (morphType == MorphologyType.Dilatation)
                    {
                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                if (spojnoscType == Spojnosc.Czterospojne)
                                {
                                    if (filterY == 0 || filterX == 0)
                                    {
                                        calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                                        if (pixelBuffer[calcOffset] > blue)
                                        {
                                            blue = pixelBuffer[calcOffset];
                                        }

                                        if (pixelBuffer[calcOffset + 1] > green)
                                        {
                                            green = pixelBuffer[calcOffset + 1];
                                        }

                                        if (pixelBuffer[calcOffset + 2] > red)
                                        {
                                            red = pixelBuffer[calcOffset + 2];
                                        }
                                    }
                                }
                                else if (spojnoscType == Spojnosc.Osmiospojne)
                                {
                                    calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                                    if (pixelBuffer[calcOffset] > blue)
                                    {
                                        blue = pixelBuffer[calcOffset];
                                    }

                                    if (pixelBuffer[calcOffset + 1] > green)
                                    {
                                        green = pixelBuffer[calcOffset + 1];
                                    }

                                    if (pixelBuffer[calcOffset + 2] > red)
                                    {
                                        red = pixelBuffer[calcOffset + 2];
                                    }
                                }
                            }
                        }
                    }
                    else if (morphType == MorphologyType.Erosion)
                    {
                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                if (spojnoscType == Spojnosc.Czterospojne)
                                {
                                    if (filterY == 0 || filterX == 0)
                                    {
                                        calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                                        if (pixelBuffer[calcOffset] < blue)
                                        {
                                            blue = pixelBuffer[calcOffset];
                                        }

                                        if (pixelBuffer[calcOffset + 1] < green)
                                        {
                                            green = pixelBuffer[calcOffset + 1];
                                        }

                                        if (pixelBuffer[calcOffset + 2] < red)
                                        {
                                            red = pixelBuffer[calcOffset + 2];
                                        }
                                    }
                                }
                                else if (spojnoscType == Spojnosc.Osmiospojne)
                                {
                                    calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                                    if (pixelBuffer[calcOffset] < blue)
                                    {
                                        blue = pixelBuffer[calcOffset];
                                    }

                                    if (pixelBuffer[calcOffset + 1] < green)
                                    {
                                        green = pixelBuffer[calcOffset + 1];
                                    }

                                    if (pixelBuffer[calcOffset + 2] < red)
                                    {
                                        red = pixelBuffer[calcOffset + 2];
                                    }
                                }
                            }
                        }
                    }

                    if (applyBlue == false)
                    {
                        blue = pixelBuffer[byteOffset];
                    }

                    if (applyGreen == false)
                    {
                        green = pixelBuffer[byteOffset + 1];
                    }

                    if (applyRed == false)
                    {
                        red = pixelBuffer[byteOffset + 2];
                    }

                    resultBuffer[byteOffset] = blue;
                    resultBuffer[byteOffset + 1] = green;
                    resultBuffer[byteOffset + 2] = red;
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }

        public static Bitmap OpenMorphologyFilter(Bitmap sourceBitmap, int matrixSize, Spojnosc spojnosc, bool applyBlue = true, bool applyGreen = true, bool applyRed = true)
        {
            Bitmap resultBitmap = DilateAndErodeFilter(sourceBitmap, matrixSize, MorphologyType.Erosion, spojnosc, applyBlue, applyGreen, applyRed);
            resultBitmap = DilateAndErodeFilter(sourceBitmap, matrixSize, MorphologyType.Dilatation, spojnosc, applyBlue, applyGreen, applyRed);
            return resultBitmap;
        }


        public static Bitmap CloseMorphologyFilter(Bitmap sourceBitmap, int matrixSize, Spojnosc spojnosc, bool applyBlue = true, bool applyGreen = true, bool applyRed = true)
        {
            Bitmap resultBitmap = DilateAndErodeFilter(sourceBitmap, matrixSize, MorphologyType.Dilatation, spojnosc, applyBlue, applyGreen, applyRed);
            resultBitmap = DilateAndErodeFilter(sourceBitmap, matrixSize, MorphologyType.Erosion, spojnosc, applyBlue, applyGreen, applyRed);
            return resultBitmap;
        }

        public static Bitmap Binarize(Bitmap sourceBitmap)
        {
            Bitmap bmp = (Bitmap)sourceBitmap.Clone();
            FastBitmap lockBitmap = new FastBitmap(bmp);
            lockBitmap.LockImage();

            for (int i = 0; i < sourceBitmap.Width; i++)
                for (int j = 0; j < sourceBitmap.Height; j++)
                {
                    if (lockBitmap.GetPixel(i, j).R > 60 || lockBitmap.GetPixel(i, j).R > 160)
                        lockBitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                        lockBitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }

            lockBitmap.UnlockImage();
            return bmp;
        }

        public static Bitmap ConvolutionFilter(this Bitmap sourceBitmap, int[,] filterMask, bool linearFilter = false, bool isHsv = false, int metoda = 3)
        {
            int divisor = 0;
            foreach (int value in filterMask)
                divisor += value;
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;


            double h = 0.0;
            double s = 0.0;
            double v = 0.0;
            double sum_h = 0.0;
            double sum_s = 0.0;
            double sum_v = 0.0;

            //double blueW = 0.0;
            //double greenW = 0.0;
            //double redW = 0.0;

            double blueMAX = 255;
            double greenMAX = 255;
            double redMAX = 255;

            double blueMIN = 0.0;
            double greenMIN = 0.0;
            double redMIN = 0.0;


            double vMAX = 255;
            double sMAX = 255;
            double hMAX = 255;

            double vMIN = 0.0;
            double sMIN = 0.0;
            double hMIN = 0.0;

            int filterWidth = filterMask.GetLength(1);
            int filterHeight = filterMask.GetLength(0);

            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            #region Obliczenia max i min do skalowania

            if (metoda == 1)
            {
                for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                    {
                        byteOffset = offsetY * sourceData.Stride + offsetX * 4;

                        blue = 0;
                        green = 0;
                        red = 0;


                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                                if (isHsv)
                                {
                                    hsv.ColorToHSV(Color.FromArgb(pixelBuffer[calcOffset + 2], pixelBuffer[calcOffset + 1], pixelBuffer[calcOffset]), out h, out s, out v);
                                    //h = (h * 255) / 360;
                                    //sum_h += h * filterMask[filterY + filterOffset, filterX + filterOffset];
                                    //s = s * 255;
                                    //sum_s += s * filterMask[filterY + filterOffset, filterX + filterOffset];
                                    v = v * 255;
                                    sum_v += v * filterMask[filterY + filterOffset, filterX + filterOffset];
                                }
                                else
                                {
                                    blue += (double)(pixelBuffer[calcOffset]) * filterMask[filterY + filterOffset, filterX + filterOffset];
                                    green += (double)(pixelBuffer[calcOffset + 1]) * filterMask[filterY + filterOffset,
                                                 filterX + filterOffset];
                                    red += (double)(pixelBuffer[calcOffset + 2]) * filterMask[filterY + filterOffset,
                                               filterX + filterOffset];
                                }

                            }
                        }

                        if (linearFilter)
                        {
                            if (isHsv)
                            {
                                sum_h = sum_h / divisor;
                                sum_s = sum_s / divisor;
                                sum_v = sum_v / divisor;
                            }
                            else
                            {
                                blue /= divisor;
                                green /= divisor;
                                red /= divisor;
                            }
                        }
                        if (isHsv)
                        {
                            if (sum_v > vMAX) vMAX = sum_v;
                            if (sum_v < vMIN) vMIN = sum_v;

                            if (sum_s > sMAX) sMAX = sum_s;
                            if (sum_s < sMIN) sMIN = sum_s;

                            if (sum_h > hMAX) hMAX = sum_h;
                            if (sum_h < hMIN) hMIN = sum_h;
                        }
                        else
                        {

                            if (blue > blueMAX) blueMAX = blue;
                            if (blue < blueMIN) blueMIN = blue;

                            if (green > greenMAX) greenMAX = green;
                            if (green < greenMIN) greenMIN = green;

                            if (red > redMAX) redMAX = red;
                            if (red < redMIN) redMIN = red;
                        }
                    }
                }
            }

            #endregion

            filterOffset = (filterWidth - 1) / 2;
            calcOffset = 0;
            byteOffset = 0;
            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    if (isHsv)
                    {
                        h = 0.0;
                        s = 0.0;
                        v = 0.0;
                        sum_h = 0.0;
                        sum_s = 0.0;
                        sum_v = 0.0;
                    }
                    else
                    {
                        blue = 0;
                        green = 0;
                        red = 0;
                    }
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            if (isHsv)
                            {
                                hsv.ColorToHSV(Color.FromArgb(pixelBuffer[calcOffset + 2], pixelBuffer[calcOffset + 1], pixelBuffer[calcOffset]), out h, out s, out v);
                                v = v * 255;
                                sum_v += v * filterMask[filterY + filterOffset, filterX + filterOffset];
                                //s = s * 255;
                                //sum_s += s * filterMask[filterY + filterOffset, filterX + filterOffset];
                                //h = (h * 255) / 360;
                                //sum_h += h * filterMask[filterY + filterOffset, filterX + filterOffset];


                            }
                            else
                            {
                                blue += (double)(pixelBuffer[calcOffset]) * filterMask[filterY + filterOffset,
                                     filterX + filterOffset];
                                green += (double)(pixelBuffer[calcOffset + 1]) * filterMask[filterY + filterOffset,
                                          filterX + filterOffset];
                                red += (double)(pixelBuffer[calcOffset + 2]) * filterMask[filterY + filterOffset,
                                        filterX + filterOffset];
                            }
                        }
                    }

                    if (linearFilter)
                    {
                        if (isHsv)
                        {
                            sum_h = sum_h / divisor;
                            sum_s = sum_s / divisor;
                            sum_v = sum_v / divisor;
                        }
                        else
                        {
                            blue /= divisor;
                            green /= divisor;
                            red /= divisor;
                        }
                    }

                    if (isHsv)
                    {
                        GetOnScale(metoda, ref sum_v, ref sum_h, ref sum_s, vMIN, vMAX, hMIN, hMAX, sMIN, sMAX);
                        Color color = hsv.ColorFromHSV(h, s, sum_v / 255);
                        blue = color.B;
                        green = color.G;
                        red = color.R;
                    }
                    else
                    {
                        GetOnScale(metoda, ref blue, ref green, ref red, blueMIN, blueMAX, greenMIN, greenMAX, redMIN, redMAX);
                    }

                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Bitmap ConvolutionFilter(this Bitmap sourceImage, int[,] xkernel, int[,] ykernel, double factor = 1, int bias = 0, bool grayscale = false, bool isHsv = false, int metoda = 3)
        {

            //Image dimensions stored in variables for convenience
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            //Lock source image bits into system memory
            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //Get the total number of bytes in your image - 32 bytes per pixel x image width x image height -> for 32bpp images
            int bytes = srcData.Stride * srcData.Height;

            //Create byte arrays to hold pixel information of your image
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            //Get the address of the first pixel data
            IntPtr srcScan0 = srcData.Scan0;

            //Copy image data to one of the byte arrays
            Marshal.Copy(srcScan0, pixelBuffer, 0, bytes);

            //Unlock bits from system memory -> we have all our needed info in the array
            sourceImage.UnlockBits(srcData);


            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            double blueMAX = 255;
            double greenMAX = 255;
            double redMAX = 255;

            double blueMIN = 0.0;
            double greenMIN = 0.0;
            double redMIN = 0.0;

            double vMAX = 255;
            double sMAX = 255;
            double hMAX = 255;

            double vMIN = 0.0;
            double sMIN = 0.0;
            double hMIN = 0.0;
            double h = 0.0f, s = 0.0f, v = 0.0f;


            //Convert your image to grayscale if necessary
            if (grayscale)
            {
                float rgb = 0;
                for (int i = 0; i < pixelBuffer.Length; i += 4)
                {
                    rgb = pixelBuffer[i] * .21f;
                    rgb += pixelBuffer[i + 1] * .71f;
                    rgb += pixelBuffer[i + 2] * .071f;
                    pixelBuffer[i] = (byte)rgb;
                    pixelBuffer[i + 1] = pixelBuffer[i];
                    pixelBuffer[i + 2] = pixelBuffer[i];
                    pixelBuffer[i + 3] = 255;
                }
            }

            //Create variable for pixel data for each kernel
            double xr = 0.0;
            double xg = 0.0;
            double xb = 0.0;
            double yr = 0.0;
            double yg = 0.0;
            double yb = 0.0;
            double rt = 0.0;
            double gt = 0.0;
            double bt = 0.0;

            double xsum_h = 0.0;
            double xsum_s = 0.0;
            double xsum_v = 0.0;
            double ysum_h = 0.0;
            double ysum_s = 0.0;
            double ysum_v = 0.0;
            double sum_h = 0.0;
            double sum_s = 0.0;
            double sum_v = 0.0;

            int filterOffset = 1;
            int calcOffset = 0;
            int byteOffset = 0;

            #region Obliczenia max i min do skalowania

            if (metoda == 1)
            {
                for (int OffsetY = filterOffset; OffsetY < height - filterOffset; OffsetY++)
                {
                    for (int OffsetX = filterOffset; OffsetX < width - filterOffset; OffsetX++)
                    {
                        //reset rgb values to 0
                        xr = xg = xb = yr = yg = yb = 0;
                        rt = gt = bt = 0.0;
                        h = s = v = 0.0f;
                        sum_h = 0.0;
                        sum_s = 0.0;
                        sum_v = 0.0;
                        xsum_v = ysum_v =  xsum_s = ysum_s =  xsum_h = ysum_h = 0.0;


                        //position of the kernel center pixel
                        byteOffset = OffsetY * srcData.Stride + OffsetX * 4;
                        //kernel calculations
                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                calcOffset = byteOffset + filterX * 4 + filterY * srcData.Stride;
                                if (isHsv)
                                {
                                    hsv.ColorToHSV(Color.FromArgb(pixelBuffer[calcOffset + 2], pixelBuffer[calcOffset + 1], pixelBuffer[calcOffset]), out h, out s, out v);
                                    v = v * 255;
                                    xsum_v += v * xkernel[filterY + filterOffset, filterX + filterOffset];
                                    ysum_v += v * ykernel[filterY + filterOffset, filterX + filterOffset];
                                    //s = s * 255;
                                    //xsum_s += s * xkernel[filterY + filterOffset, filterX + filterOffset];
                                    //ysum_s += s * ykernel[filterY + filterOffset, filterX + filterOffset];
                                    //h = (h * 255) / 360;
                                    //xsum_h += h * xkernel[filterY + filterOffset, filterX + filterOffset];
                                    //ysum_h += h * ykernel[filterY + filterOffset, filterX + filterOffset];
                                }
                                else
                                {
                                    xb += (double)(pixelBuffer[calcOffset]) *
                                          xkernel[filterY + filterOffset, filterX + filterOffset];
                                    xg += (double)(pixelBuffer[calcOffset + 1]) *
                                          xkernel[filterY + filterOffset, filterX + filterOffset];
                                    xr += (double)(pixelBuffer[calcOffset + 2]) *
                                          xkernel[filterY + filterOffset, filterX + filterOffset];
                                    yb += (double)(pixelBuffer[calcOffset]) *
                                          ykernel[filterY + filterOffset, filterX + filterOffset];
                                    yg += (double)(pixelBuffer[calcOffset + 1]) *
                                          ykernel[filterY + filterOffset, filterX + filterOffset];
                                    yr += (double)(pixelBuffer[calcOffset + 2]) *
                                          ykernel[filterY + filterOffset, filterX + filterOffset];
                                }

                            }
                        }
                        if (isHsv)
                        {
                            sum_v = Math.Sqrt((xsum_v * xsum_v) + (ysum_v * ysum_v));
                            sum_s = Math.Sqrt((xsum_s * xsum_s) + (ysum_s * ysum_s));
                            sum_h = Math.Sqrt((xsum_h * xsum_h) + (ysum_h * ysum_h));
                            //sum_v = Math.Abs(xsum_v) + Math.Abs(ysum_v);
                            //sum_s = Math.Abs(xsum_s) + Math.Abs(ysum_s);
                            //sum_h = Math.Abs(xsum_h) + Math.Abs(ysum_h);

                            sum_v = factor * sum_v + bias;
                            sum_s = factor * sum_s + bias;
                            sum_h = factor * sum_h + bias;

                            if (sum_v > vMAX) vMAX = sum_v;
                            if (sum_v < vMIN) vMIN = sum_v;

                            if (sum_s > sMAX) sMAX = sum_s;
                            if (sum_s < sMIN) sMIN = sum_s;

                            if (sum_h > hMAX) hMAX = sum_h;
                            if (sum_h < hMIN) hMIN = sum_h;


                        }
                        else
                        {
                            //total rgb values for this pixel
                            bt = Math.Sqrt((xb * xb) + (yb * yb));
                            gt = Math.Sqrt((xg * xg) + (yg * yg));
                            rt = Math.Sqrt((xr * xr) + (yr * yr));
                            //bt = Math.Abs(xb) + Math.Abs(yb);
                            //gt = Math.Abs(xg) + Math.Abs(yg);
                            //rt = Math.Abs(xr) + Math.Abs(yr);
                            bt = factor * bt + bias;
                            gt = factor * gt + bias;
                            rt = factor * rt + bias;

                            if (bt > blueMAX) blueMAX = bt;
                            if (bt < blueMIN) blueMIN = bt;

                            if (gt > greenMAX) greenMAX = gt;
                            if (gt < greenMIN) greenMIN = gt;

                            if (rt > redMAX) redMAX = rt;
                            if (rt < redMIN) redMIN = rt;
                        }
                    }
                }
            }

            #endregion


            //This is how much your center pixel is offset from the border of your kernel
            //Sobel is 3x3, so center is 1 pixel from the kernel border
            filterOffset = 1;
            calcOffset = 0;
            byteOffset = 0;

            //Start with the pixel that is offset 1 from top and 1 from the left side
            //this is so entire kernel is on your image
            for (int OffsetY = filterOffset; OffsetY < height - filterOffset; OffsetY++)
            {
                for (int OffsetX = filterOffset; OffsetX < width - filterOffset; OffsetX++)
                {
                    //reset rgb values to 0
                    xr = xg = xb = yr = yg = yb = 0;
                    rt = gt = bt = 0.0;
                    sum_h = 0.0;
                    sum_s = 0.0;
                    sum_v = 0.0;
                    xsum_v = ysum_v =  xsum_s = ysum_s =  xsum_h = ysum_h = 0.0;

                    //position of the kernel center pixel
                    byteOffset = OffsetY * srcData.Stride + OffsetX * 4;



                    //kernel calculations
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + filterX * 4 + filterY * srcData.Stride;
                            if (isHsv)
                            {
                                hsv.ColorToHSV(Color.FromArgb(pixelBuffer[calcOffset + 2], pixelBuffer[calcOffset + 1], pixelBuffer[calcOffset]), out h, out s, out v);
                                v = v * 255;
                                xsum_v += v * xkernel[filterY + filterOffset, filterX + filterOffset];
                                ysum_v += v * ykernel[filterY + filterOffset, filterX + filterOffset];
                                //s = s * 255;
                                //xsum_s += s * xkernel[filterY + filterOffset, filterX + filterOffset];
                                //ysum_s += s * ykernel[filterY + filterOffset, filterX + filterOffset];
                                //h = (h * 255) / 360;
                                //xsum_h += h * xkernel[filterY + filterOffset, filterX + filterOffset];
                                //ysum_h += h * ykernel[filterY + filterOffset, filterX + filterOffset];
                            }
                            else
                            {
                                xb += (double)(pixelBuffer[calcOffset]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                                xg += (double)(pixelBuffer[calcOffset + 1]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                                xr += (double)(pixelBuffer[calcOffset + 2]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                                yb += (double)(pixelBuffer[calcOffset]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                                yg += (double)(pixelBuffer[calcOffset + 1]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                                yr += (double)(pixelBuffer[calcOffset + 2]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                            }

                        }
                    }

                    if (isHsv)
                    {
                        sum_v = Math.Sqrt((xsum_v * xsum_v) + (ysum_v * ysum_v));
                        //sum_s = Math.Sqrt((xsum_s * xsum_s) + (ysum_s * ysum_s));
                        //sum_h = Math.Sqrt((xsum_h * xsum_h) + (ysum_h * ysum_h));
                        //sum_v = Math.Abs(xsum_v) + Math.Abs(ysum_v);
                        //sum_s = Math.Abs(xsum_s) + Math.Abs(ysum_s);
                        //sum_h = Math.Abs(xsum_h) + Math.Abs(ysum_h);
                        sum_v = factor * sum_v + bias;
                        sum_s = factor * sum_s + bias;
                        sum_h = factor * sum_h + bias;
                        GetOnScale(metoda, ref sum_v, ref sum_h, ref sum_s, vMIN, vMAX, hMIN, hMAX, sMIN, sMAX);
                        Color color = hsv.ColorFromHSV(h, s, sum_v / 255);
                        red = color.R;
                        green = color.G;
                        blue = color.B;
                    }
                    else
                    {
                        //total rgb values for this pixel
                        bt = Math.Sqrt((xb * xb) + (yb * yb));
                        gt = Math.Sqrt((xg * xg) + (yg * yg));
                        rt = Math.Sqrt((xr * xr) + (yr * yr));

                        //bt = Math.Abs(xb) + Math.Abs(yb);
                        //gt = Math.Abs(xg) + Math.Abs(yg);
                        //rt = Math.Abs(xr) + Math.Abs(yr);

                        blue = factor * bt + bias;
                        green = factor * gt + bias;
                        red = factor * rt + bias;

                        GetOnScale(metoda, ref blue, ref green, ref red, blueMIN, blueMAX, greenMIN, greenMAX, redMIN, redMAX);

                    }
                    //set new data in the other byte array for your image data
                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            //Create new bitmap which will hold the processed data
            Bitmap resultImage = new Bitmap(width, height);

            //Lock bits into system memory
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            //Copy from byte array that holds processed data to bitmap
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            //Unlock bits from system memory
            resultImage.UnlockBits(resultData);

            //Return processed image
            return resultImage;
        }
        
        private static void GetOnScale(int metoda, ref double param1, ref double param2, ref double param3,
            double param1MIN, double param1MAX, double param2MIN, double param2MAX, double param3MIN, double param3MAX)
        {
            if (metoda == 1)
            {
                {
                    param1 = (((param1 - param1MIN) / (param1MAX - param1MIN)) * 255);
                    param2 = (((param2 - param2MIN) / (param2MAX - param2MIN)) * 255);
                    param3 = (((param3 - param3MIN) / (param3MAX - param3MIN)) * 255);

                    //if (param1 > 255)
                    //{
                    //    param1 = 255;
                    //}
                    //else if (param1 < 0)
                    //{
                    //    param1 = 0;
                    //}

                    //if (param2 > 255)
                    //{
                    //    param2 = 255;
                    //}
                    //else if (param2 < 0)
                    //{
                    //    param2 = 0;
                    //}

                    //if (param3 > 255)
                    //{
                    //    param3 = 255;
                    //}
                    //else if (param3 < 0)
                    //{
                    //    param3 = 0;
                    //}
                }
            }

            if (metoda == 2)
            {
                if (param1 < 0)
                {
                    param1 = 0;
                }
                else if (param1 > 255)
                {
                    param1 = 255;
                }
                else param1 = 127;

                if (param2 < 0)
                {
                    param2 = 0;
                }
                else if (param2 > 255)
                {
                    param2 = 255;
                }
                else param2 = 127;

                if (param3 < 0)
                {
                    param3 = 0;
                }
                else if (param3 > 255)
                {
                    param3 = 255;
                }
                else param3 = 127;
            }

            if (metoda == 3)
            {
                {
                    if (param1 > 255)
                    {
                        param1 = 255;
                    }
                    else if (param1 < 0)
                    {
                        param1 = 0;
                    }

                    if (param2 > 255)
                    {
                        param2 = 255;
                    }
                    else if (param2 < 0)
                    {
                        param2 = 0;
                    }

                    if (param3 > 255)
                    {
                        param3 = 255;
                    }
                    else if (param3 < 0)
                    {
                        param3 = 0;
                    }
                }
            }

        }
    }
}
