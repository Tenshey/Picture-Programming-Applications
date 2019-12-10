using System.Drawing;
using System.Windows;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class JednoargumentoweWindow
    {

        public JednoargumentoweViewModel jednoargViewModel;
        public JednoargumentoweWindow(Bitmap bmp, int value)
        {
            jednoargViewModel = new JednoargumentoweViewModel(bmp, value);
            InitializeComponent();
            DataContext = jednoargViewModel;
        }

        private void FirstValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (jednoargViewModel.FirstValue > jednoargViewModel.SecondValue)
            {
                jednoargViewModel.SecondValue = jednoargViewModel.FirstValue + 1;
            }

            if (jednoargViewModel.ImgSourceBefore != null)
            {
                jednoargViewModel.FillWindow(jednoargViewModel.WhichMethod);
            }
        }

        private void SecondValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (jednoargViewModel.FirstValue > jednoargViewModel.SecondValue)
            {
                jednoargViewModel.FirstValue = jednoargViewModel.SecondValue - 1;
            }

            if (jednoargViewModel.ImgSourceBefore != null)
            {
                jednoargViewModel.FillWindow(jednoargViewModel.WhichMethod);
            }
        }

        private void CheckBoxRGB_Checked(object sender, RoutedEventArgs e)
        {
            jednoargViewModel.FillWindow(jednoargViewModel.WhichMethod);
        }

        private void CheckBoxHSV_Checked(object sender, RoutedEventArgs e)
        {
            jednoargViewModel.FillWindow(jednoargViewModel.WhichMethod);
        }

    }
}
