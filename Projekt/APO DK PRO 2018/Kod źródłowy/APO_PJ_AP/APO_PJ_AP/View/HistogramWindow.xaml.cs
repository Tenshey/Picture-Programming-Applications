using System.Drawing;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HistogramWindow
    {
        public HistogramViewModel histogramViewModel;
        public HistogramWindow(Bitmap bmp, int value)
        {
            histogramViewModel = new HistogramViewModel(bmp, value);
            InitializeComponent();
            DataContext = histogramViewModel;
        }

        private void ImageUserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
