using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace APO_PJ_AP.ViewModel
{
    class CompareViewModel : ViewModelBase
    {
        #region Variables
        private PointCollection _redPointCollectionBefore;
        public PointCollection RedPointCollectionBefore
        {
            get => _redPointCollectionBefore;
            set { Set(() => RedPointCollectionBefore, ref _redPointCollectionBefore, value); }
        }

        
        private PointCollection _redPointCollectionAfter;
        public PointCollection RedPointCollectionAfter
        {
            get => _redPointCollectionAfter;
            set { Set(() => RedPointCollectionAfter, ref _redPointCollectionAfter, value); }
        }

        
        #endregion

        public CompareViewModel(PointCollection redPointCollectionBefore, PointCollection redPointCollectionAfter)
        {
            RedPointCollectionBefore = redPointCollectionBefore;
            RedPointCollectionAfter = redPointCollectionAfter;
        }
    }
}
