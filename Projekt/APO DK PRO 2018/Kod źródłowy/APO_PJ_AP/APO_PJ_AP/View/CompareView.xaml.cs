using System.Windows.Media;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for CompareView.xaml
    /// </summary>
    public partial class CompareView
    {
        public CompareView(PointCollection redPointCollectionBefore, PointCollection redPointCollectionAfter)
        {
            var compareVm = new CompareViewModel(redPointCollectionBefore, redPointCollectionAfter);
            InitializeComponent();
            DataContext = compareVm;
        }
    }
}
