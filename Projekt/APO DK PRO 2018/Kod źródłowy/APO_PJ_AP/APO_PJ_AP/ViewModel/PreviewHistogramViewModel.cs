using System.Windows.Input;
using APO_PJ_AP.Model;
using GalaSoft.MvvmLight;
using OxyPlot;

namespace APO_PJ_AP.ViewModel
{
    class PreviewHistogramViewModel : ViewModelBase
    {

        #region ICommand
        public ICommand MethodCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region Variables
        private int[,] _histogramValues;

        public int[,] HistogramValues
        {
            get { return _histogramValues; }
            set
            {
                _histogramValues = value;

            }
        }
        private int _whichMethod;
        public int WhichMethod
        {
            get => _whichMethod;
            set
            {
                Set(() => WhichMethod, ref _whichMethod, value);
            }
        }
        public PlotModel HistogramModel { get; private set; }
        public PlotModel HistogramModelR { get; private set; }
        public PlotModel HistogramModelG { get; private set; }
        public PlotModel HistogramModelB { get; private set; }

        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set
            {
                Set(() => TitleText, ref _titleText, value);
            }
        }
        #endregion

        #region Constructor
        public PreviewHistogramViewModel(int[,] histogramValues, int value)
        {
            MethodCommand = new RelayCommand(ExecuteMethodCommand, CanExecuteMethodCommand);
            HistogramModel = new PlotModel { LegendOrientation = LegendOrientation.Vertical };
            HistogramModelR = new PlotModel { LegendOrientation = LegendOrientation.Vertical };
            HistogramModelG = new PlotModel { LegendOrientation = LegendOrientation.Vertical };
            HistogramModelB = new PlotModel { LegendOrientation = LegendOrientation.Vertical };

            HistogramValues = histogramValues;
            FillWindow(value, 0);
        }
        #endregion

        public void FillWindow(int value, int wariant)
        {
            OxyColor color1 = OxyColors.Red;
            OxyColor color2 = OxyColors.Green;
            OxyColor color3 = OxyColors.Blue;

            string label1 = "Red";
            string label2 = "Green";
            string label3 = "Blue";
            switch (value)
            {
                case 0:
                    TitleText = "Histogram";
                    color1 = OxyColors.Aquamarine;
                    break;
                case 1:
                    TitleText = "Histogram RGB";
                    color1 = OxyColors.Red;
                    color2 = OxyColors.Green;
                    color3 = OxyColors.Blue;
                    break;
                case 2:
                    TitleText = "Histogram HSV";
                    color1 = OxyColors.Aqua;
                    color2 = OxyColors.DarkOrange;
                    color3 = OxyColors.MediumSlateBlue;
                    label1 = "Hue";
                    label2 = "Saturation";
                    label3 = "Value";
                    break;
            }
            WhichMethod = value;
            if (wariant == 1)
            {
                int[,] histogramValues = HistogramValues;
                var histogramModel = HistogramModel;
                if (WhichMethod == 0)
                    OxyPlotHelper.Plot1Seria("Poziom", "Ilość", color1, histogramModel, 0, histogramValues);
                else
                {
                    OxyPlotHelper.Plot3Serie("Poziom", "Ilość", color1, color2, color3, label1, label2, label3, histogramModel, histogramValues);

                }
                OxyPlotHelper.Plot1Seria("Poziom", "Ilość", color1, HistogramModelR, 0, histogramValues);
                OxyPlotHelper.Plot1Seria("Poziom", "Ilość", color2, HistogramModelG, 1, histogramValues);
                OxyPlotHelper.Plot1Seria("Poziom", "Ilość", color3, HistogramModelB, 2, histogramValues);
            }
            else
            {
                int[,] histogramValues = HistogramValues;
                PlotModel histogramModel = HistogramModel;
                bool chkR = true;
                bool chkG = true;
                bool chkB = true;

                if (WhichMethod == 0)
                    OxyPlotHelper.Plot1SeriaColumn("Poziom", "Ilość", color1, histogramValues, "", histogramModel, 0);
                else
                {
                    OxyPlotHelper.Plot3SeriaKolumnowy("Poziom", "Ilość", color1, color2, color3, histogramValues, label1, label2, label3, histogramModel, chkR, chkG, chkB);
                }

                OxyPlotHelper.Plot1SeriaColumn("Poziom", "Ilość", color1, histogramValues, label1, HistogramModelR, 0);
                OxyPlotHelper.Plot1SeriaColumn("Poziom", "Ilość", color2, histogramValues, label2, HistogramModelG, 1);
                OxyPlotHelper.Plot1SeriaColumn("Poziom", "Ilość", color3, histogramValues, label3, HistogramModelB, 2);
            }
        }

        public void ExecuteMethodCommand(object parameter)
        {
            if (parameter != null)
            {
                int value = (int)parameter;
                FillWindow(value, 0);
            }
        }

        public bool CanExecuteMethodCommand(object parameter)
        {
            return true;
        }
    }
}
