using System.Drawing;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for WieloargumentoweWindow.xaml
    /// </summary>
    public partial class WieloargumentoweWindow
    {
        public WieloargumentoweViewModel WieloargViewModel = new WieloargumentoweViewModel();
        public WieloargumentoweWindow(Bitmap bmp)
        {
            WieloargViewModel.FirstImageBmp = bmp;
            InitializeComponent();
            DataContext = WieloargViewModel;
        }

        public WieloargumentoweWindow()
        {
            InitializeComponent();
 
            DataContext = WieloargViewModel;
        }
    }
}
