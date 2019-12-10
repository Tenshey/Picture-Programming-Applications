using System.Drawing;
using System.Drawing.Imaging;

namespace APO_PJ_AP
{
    enum ClrChannel
    {Blue, Green, Red, Gray}
	
    public class ColorRGB
	{
	//	Image m_Img;

	    public Bitmap GetChannel24 (Bitmap Obmp, Bitmap Nbmp, int iColor )	// [in] color plane to get
		{
			//Bitmap Obmp = (Bitmap)m_Img;
			Nbmp = new Bitmap ( Obmp.Width, Obmp.Height, PixelFormat.Format32bppArgb); //Format8bppIndexed );
		//	ColorPalette pal = Nbmp.Palette; 
		//	for ( int i = 0; i < 256; i ++ )
		//		pal.Entries[i] = Color.FromArgb(i, i, i);;		
		//    	Nbmp.Palette = pal; // The crucial statement 
				
                SplitRGB_24bpp (Obmp, Nbmp, iColor);
	
			return Nbmp;
		}




		private void SplitRGB_24bpp (Bitmap Obmp, Bitmap Nbmp, int iColor )
		{
            BitmapData Odata = Obmp.LockBits( new Rectangle( 0 , 0 , Obmp.Width , Obmp.Height ) , ImageLockMode.ReadWrite  , PixelFormat.Format24bppRgb  );
			BitmapData Ndata = Nbmp.LockBits( new Rectangle( 0 , 0 , Nbmp.Width , Nbmp.Height ) , ImageLockMode.ReadWrite  , PixelFormat.Format8bppIndexed  );

			unsafe
			{ 
				byte* Optr = ( byte* )( Odata.Scan0 );
				byte* Nptr = ( byte* )( Ndata.Scan0 ); 
 
				for ( int y = 0; y < Obmp.Height; y ++ )
				{
					for ( int x = 0; x < Obmp.Width; x ++ )
					{
						byte b = *Optr; Optr ++;
						byte g = *Optr; Optr ++;
						byte r = *Optr; Optr ++;

						switch ( iColor )
						{
							case (int)ClrChannel.Blue:
								*Nptr = b; Nptr ++;
								break;
							case (int)ClrChannel.Green:
								*Nptr = g; Nptr ++;
								break;
							case (int)ClrChannel.Red:
								*Nptr = r; Nptr ++;
								break;
							case (int)ClrChannel.Gray:
								*Nptr = (byte)(( b + (double)g + r ) / 3.0); Nptr ++;
								break;
						};
					}
					Optr += Odata.Stride - Odata.Width*3;
					Nptr += Ndata.Stride - Ndata.Width;
				}
				Obmp.UnlockBits(Odata);
				Nbmp.UnlockBits(Ndata);
			}
           }
		


		private void SplitRGB_default (Bitmap Obmp, Bitmap Nbmp, int iColor )
		{
			BitmapData Ndata = Nbmp.LockBits( new Rectangle( 0 , 0 , Nbmp.Width , Nbmp.Height ) , ImageLockMode.ReadWrite  , PixelFormat.Format8bppIndexed  );

			unsafe
			{ 
				byte* Nptr = ( byte* )( Ndata.Scan0 ); 
 
				for ( int y = 0; y < Obmp.Height; y ++ )
				{
					for ( int x = 0; x < Obmp.Width; x ++ )
					{
						Color clr = Obmp.GetPixel(x,y);

						switch ( iColor )
						{
							case (int)ClrChannel.Blue:
								*Nptr = clr.B; Nptr ++;
								break;

							case (int)ClrChannel.Green:
								*Nptr = clr.G; Nptr ++;
								break;

							case (int)ClrChannel.Red:
								*Nptr = clr.R; Nptr ++;
								break;

							case (int)ClrChannel.Gray:
								*Nptr = (byte)(( clr.B + (double)clr.G + clr.R ) / 3.0); Nptr ++;
								break;
						};
					}
					Nptr += Ndata.Stride - Ndata.Width;
				}
				Nbmp.UnlockBits(Ndata);
                }
             }
			

  }
}