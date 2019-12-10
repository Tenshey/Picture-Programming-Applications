using System.Drawing;
using System.Windows;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow
    {
        public PreviewViewModel previewViewModel;

        public PreviewWindow(Bitmap bmp, int value)
        {
            InitializeComponent();
            previewViewModel = new PreviewViewModel(bmp, value);
            DataContext = previewViewModel;
            switch (value)
            {
                case 1:
                    SaveButton.Visibility = Visibility.Hidden;
                    break;
                case 0:
                    SaveButton.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
