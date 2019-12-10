using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using APO_PJ_AP.Model;
using APO_PJ_AP.Operation;
using GalaSoft.MvvmLight;

namespace APO_PJ_AP.ViewModel
{
    public class HistogramViewModel : ViewModelBase
    {
        #region ICommand
        public ICommand MethodCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region Variables
        public ObservableCollection<MenuModel> MenuItems { get; set; }
        private Bitmap _imgSourceBefore;
        public Bitmap ImgSourceBefore
        {
            get => _imgSourceBefore;
            set { Set(() => ImgSourceBefore, ref _imgSourceBefore, value); }
        }

        private Bitmap _imgSourceAfter;
        public Bitmap ImgSourceAfter
        {
            get => _imgSourceAfter;
            set { Set(() => ImgSourceAfter, ref _imgSourceAfter, value); }
        }
        public bool IsSaved;
        private int _whichMethod;
        public int WhichMethod
        {
            get => _whichMethod;
            set
            {
                Set(() => WhichMethod, ref _whichMethod, value);
            }
        }
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
        public HistogramViewModel(Bitmap bmp, int value)
        {
            MethodCommand = new RelayCommand(ExecuteMethodCommand, CanExecuteMethodCommand);
            SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
            ImgSourceBefore = bmp;
            FillWindow(value);

            MenuItems = new ObservableCollection<MenuModel>
                    {
                        new MenuModel { Header = "Operacje na histogramie",
                            MenuItems = new ObservableCollection<MenuModel>
                            {
                                new MenuModel { Header = "Wygładzanie",
                                    MenuItems = new ObservableCollection<MenuModel>
                                    {
                                        new MenuModel { Header = "Metoda średnich",
                                        Index = 1,
                                        Command = MethodCommand
                                        },
                                        new MenuModel { Header = "Metoda losowych",
                                        Index = 2,
                                        Command = MethodCommand
                                        },
                                        new MenuModel { Header = "Metoda sąsiedztwa",
                                        Index = 3,
                                        Command = MethodCommand
                                        },
                                        new MenuModel { Header = "Metoda własna",
                                        Index = 4,
                                        Command = MethodCommand
                                        }
                                    }
                                },
                                new MenuModel { Header = "Rozciąganie",
                                Index = 5,
                                Command = MethodCommand
                                }
                            }
                        },
                        new MenuModel { Header = "Pomoc" }
                    };
        }
        #endregion

        public void FillWindow(int value)
        {
            using (new WaitCursor())
            {
                switch (value)
                {
                    case 1:
                        TitleText = "Histogram - Wygładzanie - Metoda średnich";
                        ImgSourceAfter = HistogramOperation.histogram_wyrownanie(ImgSourceBefore,
                            HistogramOperation.MetodyWyrownywania.Srednich);
                        break;
                    case 2:
                        TitleText = "Histogram - Wygładzanie - Metoda losowych";
                        ImgSourceAfter = HistogramOperation.histogram_wyrownanie(ImgSourceBefore,
                            HistogramOperation.MetodyWyrownywania.Losowych);
                        break;
                    case 3:
                        TitleText = "Histogram - Wygładzanie - Metoda sąsiedztwa";
                        ImgSourceAfter = HistogramOperation.histogram_wyrownanie(ImgSourceBefore,
                            HistogramOperation.MetodyWyrownywania.Sasiedztwa);
                        break;
                    case 4:
                        TitleText = "Histogram - Wygładzanie - Metoda własna";
                        ImgSourceAfter = HistogramOperation.OwnMethod(ImgSourceBefore);
                        break;
                    case 5:
                        TitleText = "Histogram - Rozciąganie";
                        ImgSourceAfter = HistogramOperation.histogram_Stretching(ImgSourceBefore);
                        break;
                }

                WhichMethod = value;
            }
        }

        public void ExecuteMethodCommand(object parameter)
        {
            if (parameter != null)
            {
                int value = (int)parameter;
                FillWindow(value);
            }
        }

        public bool CanExecuteMethodCommand(object parameter)
        {
            return true;
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
            if (ImgSourceAfter == null)
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
