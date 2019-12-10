using System.Drawing;
using System.Windows;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for OperacjeMorfologiczneWindow.xaml
    /// </summary>
    public partial class OperacjeMorfologiczneWindow : Window
    {
        public OperacjeMorfologiczneViewModel operacjeMorfologiczneViewModel;
        public OperacjeMorfologiczneWindow(Bitmap bmp, int value)
        {
            operacjeMorfologiczneViewModel = new OperacjeMorfologiczneViewModel(value);
            operacjeMorfologiczneViewModel.ImgSourceBefore = bmp;
            InitializeComponent();
            DataContext = operacjeMorfologiczneViewModel;
        }
    }
}
