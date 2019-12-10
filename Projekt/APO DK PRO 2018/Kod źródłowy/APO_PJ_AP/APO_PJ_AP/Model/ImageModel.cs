using System;
using System.Drawing;

namespace APO_PJ_AP.Model
{    public class ImageModel

    {
        private Bitmap _bmp;
        private bool _isRgb;
        private int _height;
        private int _width;
        private Color[,] _pixelsTab;
        private int[,] _rgbArray;

        public Bitmap Bmp
        {
            get => _bmp;
        }

        public bool IsRgb
        {
            get => _isRgb;
            set => _isRgb = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }
        //Przechowuje kolor każdego pixela
        public Color[,] PixelsTab
        {
            get => _pixelsTab;
            set => _pixelsTab = value;
        }
        //Przechowuje wartości RGB
        public int[,] RgbArray
        {
            get => _rgbArray;
            set => _rgbArray = value;
        }

        //Pobiera jeden kanał (R, G lub B)
        private int[] GetOneDimensionFromRgbArray(int y)
        {
            int[] result = new int[256];
            for (int i = 0; i < 256; i++)
            {
                result[i] = RgbArray[y, i];
            }
            return result;
        }
        public int[] GetRArray()
        {
            int[] result = new int[256];
            for (int i = 0; i < 256; i++)
            {
                result[i] = RgbArray[0, i];
            }
            return result;
        }
        public int[] GetGArray()
        {
            int[] result = new int[256];
            for (int i = 0; i < 256; i++)
            {
                result[i] = RgbArray[1, i];
            }
            return result;
        }
        public int[] GetBArray()
        {
            int[] result = new int[256];
            for (int i = 0; i < 256; i++)
            {
                result[i] = RgbArray[2, i];
            }
            return result;
        }
        //Wypełnia klasę danymi wczytanymi z obrazka
        public ImageModel(Bitmap bmp)
        {
            _bmp = bmp;
            //zmienić przypisywanie - po sprawdzeniu czy jest rgb (jeden pixelformat dla wszystkich obrazków)
            if (bmp.PixelFormat.ToString().Contains("Rgb"))
                IsRgb = true;

            FastBitmap lockBitmap = new FastBitmap(bmp);
            lockBitmap.LockImage();

            Height = bmp.Height;
            Width = bmp.Width;
            PixelsTab = new Color[bmp.Width, bmp.Height];
            RgbArray = new int[3, 256];

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    var color = lockBitmap.GetPixel(i, j);
                    PixelsTab[i, j] = color;
                    RgbArray[0, color.R]++;
                    RgbArray[1, color.G]++;
                    RgbArray[2, color.B]++;
 
                }

            lockBitmap.UnlockImage();
        }

        public void CountRgbArray()
        {
            FastBitmap lockBitmap = new FastBitmap(Bmp);
            lockBitmap.LockImage();

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    Color color = lockBitmap.GetPixel(i, j);
                    RgbArray[0, Convert.ToInt32(color.R)]++;
                    if (IsRgb)
                    {
                        RgbArray[1, Convert.ToInt32(color.G)]++;
                        RgbArray[2, Convert.ToInt32(color.B)]++;
                    }
                }

            lockBitmap.UnlockImage();
        }
    }
}
