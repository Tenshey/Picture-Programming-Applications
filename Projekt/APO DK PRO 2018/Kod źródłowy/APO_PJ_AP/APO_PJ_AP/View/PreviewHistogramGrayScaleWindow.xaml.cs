using System.Windows;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for PreviewHistogramWindow.xaml
    /// </summary>
    public partial class PreviewHistogramGrayScaleWindow
    {
        PreviewHistogramViewModel previewViewModel;

        public PreviewHistogramGrayScaleWindow(int[,] histogramValues, int value)
        {
            InitializeComponent();
            previewViewModel = new PreviewHistogramViewModel(histogramValues, value);
            DataContext = previewViewModel;
        }

        private void CheckBoxSlupkowy_Checked(object sender, RoutedEventArgs e)
        {
            previewViewModel?.FillWindow(previewViewModel.WhichMethod, 0);
        }

        private void CheckBoxLiniowy_Checked(object sender, RoutedEventArgs e)
        {
            previewViewModel?.FillWindow(previewViewModel.WhichMethod, 1);
        }
    }
}
