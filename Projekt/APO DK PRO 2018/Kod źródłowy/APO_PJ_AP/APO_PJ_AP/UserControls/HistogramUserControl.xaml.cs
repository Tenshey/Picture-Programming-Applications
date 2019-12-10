using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using APO_PJ_AP.View;
using OxyPlot;

namespace APO_PJ_AP.UserControls
{
    /// <summary>
    /// Interaction logic for ObrazekUserControl.xaml
    /// </summary>
    public partial class HistogramUserControl : UserControl
    {
        public static readonly DependencyProperty SourceBitmapForHistogramProperty = DependencyProperty.Register(nameof(SourceBitmapForHistogram), typeof(Bitmap), typeof(HistogramUserControl), new FrameworkPropertyMetadata(null, ValueChanged));
        public static readonly DependencyProperty PointListProperty = DependencyProperty.Register(nameof(PointList), typeof(int[,]), typeof(HistogramUserControl), new FrameworkPropertyMetadata(null, ValueChanged));

        public int[,] HistogramHsvValues { get; private set; } = new int[361, 3];
        public int[,] HistogramRgbValues { get; private set; } = new int[256, 3];

  
        public HistogramUserControl()
        {
            InitializeComponent();
            HistogramTabControl.Visibility = DesignerProperties.GetIsInDesignMode(this) ? Visibility.Visible : Visibility.Hidden;
            LiniaProfiluTabItem.Visibility = DesignerProperties.GetIsInDesignMode(this) ? Visibility.Visible : Visibility.Hidden;
        }
        public Bitmap SourceBitmapForHistogram
        {

            get => (Bitmap)GetValue(SourceBitmapForHistogramProperty);
            set => SetValue(SourceBitmapForHistogramProperty, value);
        }

        public int[,] PointList
        {
            get => (int[,])GetValue(PointListProperty);
            set => SetValue(PointListProperty, value);
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HistogramUserControl)d;

            if (e.Property.Name.Equals(nameof(SourceBitmapForHistogram)))
            {
                control.HistogramTabControl.Visibility = Visibility.Visible;
                control.Generuj_histogram(control.SourceBitmapForHistogram);
                RysujHistogram(control);
                RysujHistRGB(control);
                RysujHistHSV(control);

            }
            if (e.Property.Name.Equals(nameof(PointList)))
            {
                control.LiniaProfiluTabItem.Visibility = Visibility.Visible;
                control.LiniaProfiluTabItem.IsSelected = true;
                RysujLinieProfilu(control);

            }
        }

        private static void RysujHistogram(HistogramUserControl control)
        {
            if (control.HistogramRgbValues == null) return;
            control.HistGrayImage.Model = new PlotModel();
            OxyPlotHelper.Plot1SeriaColumn("Poziom", "Ilość", OxyColors.Aquamarine, control.HistogramRgbValues, "", control.HistGrayImage.Model, 0);

        }

        private static void RysujLinieProfilu(HistogramUserControl control)
        {
            if (control.PointList == null) return;
            control.LiniaProfiluPlot.Model = new PlotModel();
            bool chkLiniaGray = control.CheckBoxLiniaSkalaGray != null &&
                                control.CheckBoxLiniaSkalaGray.IsChecked.Value;
            bool chkLiniaRGB = control.CheckBoxLiniaRGB != null && control.CheckBoxLiniaRGB.IsChecked.Value;
            bool chkLiniaHSV = control.CheckBoxLiniaHSV != null && control.CheckBoxLiniaHSV.IsChecked.Value;

            if (chkLiniaGray)
                OxyPlotHelper.Plot1Seria("Numer piksela", "Wartość", OxyColors.Aquamarine, control.LiniaProfiluPlot.Model, 0, control.PointList);
            if (chkLiniaRGB)
            {
                OxyColor color1 = OxyColors.Red;
                OxyColor color2 = OxyColors.Green;
                OxyColor color3 = OxyColors.Blue;
                string label1 = "Red";
                string label2 = "Green";
                string label3 = "Blue";

                OxyPlotHelper.Plot3Serie("Numer piksela", "Wartość", color1, color2, color3, label1, label2, label3,
                    control.LiniaProfiluPlot.Model, control.PointList);
            }

            if (chkLiniaHSV)
            {
                OxyColor color1 = OxyColors.Aqua;
                OxyColor color2 = OxyColors.DarkOrange;
                OxyColor color3 = OxyColors.MediumSlateBlue;
                string label1 = "Hue";
                string label2 = "Saturation";
                string label3 = "Value";

                var l = control.PointList.Length / 3;
                var pointListHSV = (int[,])control.PointList.Clone();
                for (int i = 0; i < l; i++)
                {
                    hsv.ColorToHSV(Color.FromArgb(pointListHSV[i, 0], pointListHSV[i, 1], pointListHSV[i, 2]), out var h, out var s, out var v);
                    pointListHSV[i, 0] = (int)h;
                    pointListHSV[i, 1] = (int)(s * 360.0f);
                    pointListHSV[i, 2] = (int)(v * 360.0f);
                }
                OxyPlotHelper.Plot3Serie("Numer piksela", "Wartość", color1, color2, color3, label1, label2, label3,
                    control.LiniaProfiluPlot.Model, pointListHSV);
            }
        }
        private void CheckBoxRGB_Checked(object sender, RoutedEventArgs e)
        {
            RysujHistRGB(this);
        }

        private static void RysujHistRGB(HistogramUserControl control)
        {
            if (control.SourceBitmapForHistogram != null)
            {
                if (control.HistogramRgbValues == null) return;
                control.HistRGBImage.Model = new PlotModel();
                bool chkR = control.CheckBoxR.IsChecked != null && control.CheckBoxR.IsChecked.Value;
                bool chkG = control.CheckBoxG.IsChecked != null && control.CheckBoxG.IsChecked.Value;
                bool chkB = control.CheckBoxB.IsChecked != null && control.CheckBoxB.IsChecked.Value;

                OxyColor color1 = OxyColors.Red;
                OxyColor color2 = OxyColors.Green;
                OxyColor color3 = OxyColors.Blue;
                string label1 = "Red";
                string label2 = "Green";
                string label3 = "Blue";
                OxyPlotHelper.Plot3SeriaKolumnowy("Poziom", "Ilość", color1, color2, color3, control.HistogramRgbValues, label1, label2, label3,
                    control.HistRGBImage.Model, chkR, chkG, chkB);
                if (!chkR && !chkG && !chkB)
                {
                    control.CheckBoxR.IsChecked = true;
                }

            }
        }
        
        private static void RysujHistHSV(HistogramUserControl control)
        {
            if (control.SourceBitmapForHistogram != null)
            {

                if (control.HistogramHsvValues == null) return;
                control.HistHSVImage.Model = new PlotModel();
                bool chkH = control.CheckBoxH.IsChecked != null && control.CheckBoxH.IsChecked.Value;
                bool chkS = control.CheckBoxS.IsChecked != null && control.CheckBoxS.IsChecked.Value;
                bool chkV = control.CheckBoxV.IsChecked != null && control.CheckBoxV.IsChecked.Value;

                OxyColor color1 = OxyColors.Aqua;
                OxyColor color2 = OxyColors.DarkOrange;
                OxyColor color3 = OxyColors.MediumSlateBlue;
                string label1 = "Hue";
                string label2 = "Saturation";
                string label3 = "Value";
                OxyPlotHelper.Plot3SeriaKolumnowy("Poziom (skalowanie S,V z <0,1> do <0,360>)", "Ilość", color1, color2, color3, control.HistogramHsvValues, label1, label2, label3,
                    control.HistHSVImage.Model, chkH, chkS, chkV);
                if (!chkH && !chkS && !chkV)
                {
                    control.CheckBoxH.IsChecked = true;
                }
            }
        }

        private void CheckBoxHSV_Checked(object sender, RoutedEventArgs e)
        {
            RysujHistHSV(this);
        }

        private void Generuj_histogram(Bitmap bmp)
        {
            #region generuj_histogram
            FastBitmap fobrazek_bitmapa = new FastBitmap(bmp);
            HistogramRgbValues = new int[256, 3];

            fobrazek_bitmapa.LockImage();
            //Zliczanie wartości histogramu
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    HistogramRgbValues[fobrazek_bitmapa.GetPixel(x, y).R, 0]++;
                    HistogramRgbValues[fobrazek_bitmapa.GetPixel(x, y).G, 1]++;
                    HistogramRgbValues[fobrazek_bitmapa.GetPixel(x, y).B, 2]++;
                    hsv.ColorToHSV(fobrazek_bitmapa.GetPixel(x, y), out var h, out var s, out var v);
                    HistogramHsvValues[(int)(h), 0]++;
                    HistogramHsvValues[(int)(s * 360.0f), 1]++;
                    HistogramHsvValues[(int)(v * 360.0f), 2]++;
                }
            }
            fobrazek_bitmapa.UnlockImage();

            #endregion
        }

        private void ButtonHistHSV_Click(object sender, RoutedEventArgs e)
        {
            int value = 2;
            var newWindow = new PreviewHistogramWindow(HistogramHsvValues, value);
            newWindow.WindowState = WindowState.Maximized;
            newWindow.ShowDialog();
        }

        private void ButtonHistRGB_Click(object sender, RoutedEventArgs e)
        {
            int value = 1;
            var newWindow = new PreviewHistogramWindow(HistogramRgbValues, value);
            newWindow.WindowState = WindowState.Maximized;
            newWindow.ShowDialog();
        }

        private void ButtonHistGray_Click(object sender, RoutedEventArgs e)
        {
            int value = 0;
            var newWindow = new PreviewHistogramGrayScaleWindow(HistogramRgbValues, value);
            newWindow.WindowState = WindowState.Maximized;
            newWindow.ShowDialog();
        }

        private void CheckBoxLinia_Checked(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                RysujLinieProfilu(this);
        }
    }
}
