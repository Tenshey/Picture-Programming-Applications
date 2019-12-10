using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using APO_PJ_AP.Model;
using GalaSoft.MvvmLight;
using static APO_PJ_AP.Operation.ImageFilter;

namespace APO_PJ_AP.ViewModel
{
    public class OperacjeMorfologiczneViewModel : ViewModelBase
    {
        #region ICommand
        public ICommand MethodCommand { get; set; }
        public ICommand DoMethodCommand { get; set; }
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
        private int _radioButtonValue;
        public int RadioButtonValue
        {
            get => _radioButtonValue;
            set
            {
                Set(() => RadioButtonValue, ref _radioButtonValue, value);
            }
        }
        private Visibility _radioButtonVisibility;
        public Visibility RadioButtonVisibility
        {
            get => _radioButtonVisibility;
            set
            {
                Set(() => RadioButtonVisibility, ref _radioButtonVisibility, value);
            }
        }
        private Spojnosc spojnosc;
        #endregion

        #region Constructor
        public OperacjeMorfologiczneViewModel(int value)
        {
            MethodCommand = new RelayCommand(ExecuteMethodCommand, CanExecuteMethodCommand);
            DoMethodCommand = new RelayCommand(ExecuteDoMethodCommand, CanExecuteDoMethodCommand);
            SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
            FillWindow(value);

            MenuItems = new ObservableCollection<MenuModel>
                    {
                        new MenuModel { Header = "Operacje morfologiczne",
                            MenuItems = new ObservableCollection<MenuModel>
                            {
                                new MenuModel { Header = "Dylatacja",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 1,
                                    Command = MethodCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 2,
                                    Command = MethodCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Erozja",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 3,
                                    Command = MethodCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 4,
                                    Command = MethodCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Otwarcia",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 5,
                                    Command = MethodCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 6,
                                    Command = MethodCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Zamknięcia",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 7,
                                    Command = MethodCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 8,
                                    Command = MethodCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Progowanie",
                                                Index = 9,
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
                        TitleText = "Dylatacja - czterospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Czterospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 1;
                        break;
                    case 2:
                        TitleText = "Dylatacja - osmiospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Osmiospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 1;
                        break;
                    case 3:
                        TitleText = "Erozja - czterospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Czterospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 2;
                        break;
                    case 4:
                        TitleText = "Erozja - osmiospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Osmiospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 2;
                        break;
                    case 5:
                        TitleText = "Otwarcia - czterospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Czterospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 3;
                        break;
                    case 6:
                        TitleText = "Otwarcia - osmiospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Osmiospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 3;
                        break;
                    case 7:
                        TitleText = "Zamknięcia - czterospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Czterospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 4;
                        break;
                    case 8:
                        TitleText = "Zamknięcia - osmiospojne";
                        RadioButtonValue = 1;
                        spojnosc = Spojnosc.Osmiospojne;
                        RadioButtonVisibility = Visibility.Visible;
                        WhichMethod = 4;
                        break;
                    case 9:
                        TitleText = "Segmentacja Progowanie";
                        RadioButtonValue = 0;
                        RadioButtonVisibility = Visibility.Hidden;
                        WhichMethod = 5;
                        break;
                }
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

        public void ExecuteDoMethodCommand(object parameter)
        {
            int matrixSize = 0;
            switch (RadioButtonValue)
            {
                case 1:
                    matrixSize = 3;
                    break;
                case 2:
                    matrixSize = 5;
                    break;
                case 3:
                    matrixSize = 7;
                    break;
            }
            switch (WhichMethod)
            {
                case 1:
                    ImgSourceAfter = DilateAndErodeFilter(ImgSourceBefore, matrixSize, MorphologyType.Dilatation, spojnosc);
                    break;
                case 2:
                    ImgSourceAfter = DilateAndErodeFilter(ImgSourceBefore, matrixSize, MorphologyType.Erosion, spojnosc);
                    break;
                case 3:
                    ImgSourceAfter = OpenMorphologyFilter(ImgSourceBefore, matrixSize, spojnosc);
                    break;
                case 4:
                    ImgSourceAfter = CloseMorphologyFilter(ImgSourceBefore, matrixSize, spojnosc);
                    break;
                case 5:
                    ImgSourceAfter = Binarize(ImgSourceBefore);
                    break;
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

        public bool CanExecuteDoMethodCommand(object parameter)
        {
            return true;
        }
    }
}
