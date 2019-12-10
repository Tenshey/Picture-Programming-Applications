using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainViewModel2 MainVm = new MainViewModel2();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainVm;
        }
    }
}
