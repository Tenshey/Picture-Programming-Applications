using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace APO_PJ_AP
{
    public static class WPFUtilities
    {
        public static BitmapImage SetImgSourceMemoryStream(Bitmap Bmp)
        {
            Bitmap newBmp = (Bitmap)Bmp.Clone();
            using (MemoryStream memory = new MemoryStream())
            {
                newBmp.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}