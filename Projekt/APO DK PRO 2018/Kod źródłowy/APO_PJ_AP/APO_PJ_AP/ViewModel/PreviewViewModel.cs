using System.Drawing;
using System.Windows;
using System.Windows.Input;
using APO_PJ_AP.Model;
using GalaSoft.MvvmLight;
using OxyPlot;

namespace APO_PJ_AP.ViewModel
{
    public class PreviewViewModel : ViewModelBase
    {

        #region ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion


        #region Constructor
        public PreviewViewModel(Bitmap bmp, int value)
        {

            SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
            SourceBitmap = bmp;
            //FillWindow(value);

        }
        #endregion

        public bool IsSaved;
        private Bitmap _sourceBitmap;
        public Bitmap SourceBitmap
        {
            get => _sourceBitmap;
            set
            {
                Set(() => SourceBitmap, ref _sourceBitmap, value);
            }
        }

        private PlotModel _plotModelBefore;
        public PlotModel PlotModelBefore
        {
            get => _plotModelBefore;
            set { Set(() => PlotModelBefore, ref _plotModelBefore, value); }
        }
        private PlotModel _plotModelAfter;
        public PlotModel PlotModelAfter
        {
            get => _plotModelAfter;
            set { Set(() => PlotModelAfter, ref _plotModelAfter, value); }
        }
        private int[,] _pointList;
        public int[,] PointList
        {
            get => _pointList;
            set
            {
                Set(() => PointList, ref _pointList, value);
            }
        }

        public void ExecuteSaveCommand(object parameter)
        {
            if (parameter != null)
            {
                IsSaved = true;
                (parameter as Window).Close();
            }
        }

        public bool CanExecuteSaveCommand(object parameter)
        {
            if (SourceBitmap == null)
                return false;
            return true;
        }

        public void ExecuteCancelCommand(object parameter)
        {
            if (parameter != null)
                (parameter as Window).Close();
        }

        public bool CanExecuteCancelCommand(object parameter)
        {
            return true;
        }
    }
}
