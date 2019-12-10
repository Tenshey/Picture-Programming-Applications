using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using APO_PJ_AP.Model;
using APO_PJ_AP.Operation;
using GalaSoft.MvvmLight;

namespace APO_PJ_AP.ViewModel
{
    public class JednoargumentoweViewModel : ViewModelBase
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
        private string _sliderLabel;
        public string SliderLabel
        {
            get => _sliderLabel;
            set
            {
                Set(() => SliderLabel, ref _sliderLabel, value);
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
        private bool? _chkRgb;
        public bool? ChkRGB
        {
            get { return _chkRgb; }
            set { Set(() => ChkRGB, ref _chkRgb, value); }
        }

        private bool? _chkHSV;
        public bool? ChkHSV
        {
            get { return _chkHSV; }
            set { Set(() => ChkHSV, ref _chkHSV, value); }
        }

        private int _firstValue;
        public int FirstValue
        {
            get => _firstValue;
            set { Set(() => FirstValue, ref _firstValue, value); }
        }

        private int _secondValue;

        public int SecondValue
        {
            get => _secondValue;
            set { Set(() => SecondValue, ref _secondValue, value); }
        }
        private Visibility _wyborKoloruGroupBoxVisibility;
        public Visibility WyborKoloruGroupBoxVisibility
        {
            get => _wyborKoloruGroupBoxVisibility;
            set { Set(() => WyborKoloruGroupBoxVisibility, ref _wyborKoloruGroupBoxVisibility, value); }
        }
        private Visibility _firstValueVisibility;
        public Visibility FirstValueVisibility
        {
            get => _firstValueVisibility;
            set { Set(() => FirstValueVisibility, ref _firstValueVisibility, value); }
        }

        private Visibility _secondValueVisibility;


        public Visibility SecondValueVisibility
        {
            get => _secondValueVisibility;
            set { Set(() => SecondValueVisibility, ref _secondValueVisibility, value); }
        }
        #endregion

        #region Constructor
        public JednoargumentoweViewModel(Bitmap bmp, int value)
        {
            MethodCommand = new RelayCommand(ExecuteMethodCommand, CanExecuteMethodCommand);
            SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
            if (value == 3 || value==4)
            {
                FirstValue = 64;
                SecondValue = 192;
            }
            else
            {
                FirstValue = 128;
                SecondValue = 128;
            }

            ChkRGB = true;
            ChkHSV = false;
            ImgSourceBefore = bmp;
            FillWindow(value);

            MenuItems = new ObservableCollection<MenuModel>
                    {
                        new MenuModel { Header = "Operacje jednoargumentowe",
                            MenuItems = new ObservableCollection<MenuModel>
                            {
                                new MenuModel { Header = "Negacja",
                                    Index = 1,
                                    Command = MethodCommand
                                },
                                new MenuModel { Header = "Progowanie",
                                    Index = 2,
                                    Command = MethodCommand
                                },
                                new MenuModel { Header = "Progowanie z zachowaniem poziomów szarości",
                                    Index = 3,
                                    Command = MethodCommand
                                },
                                new MenuModel { Header = "Rozciąganie",
                                    Index = 4,
                                    Command = MethodCommand
                                },
                                new MenuModel { Header = "Jasność",
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
                bool checkRGB = ChkRGB != null && ChkRGB.Value;
                bool checkHSV = ChkHSV != null && ChkHSV.Value;
                WyborKoloruGroupBoxVisibility = Visibility.Visible;
                switch (value)
                {
                    case 1:
                        if (checkRGB)
                        {
                            TitleText = "Operacje jednoargumentowe - Negacja RGB";
                            ImgSourceAfter = Jednopunktowe.Negacja(ImgSourceBefore);
                        }
                        if (checkHSV)
                        {
                            TitleText = "Operacje jednoargumentowe - Negacja HSV";
                            ImgSourceAfter = Jednopunktowe.NegacjaHSV(ImgSourceBefore);
                        }
                        SliderLabel = "";
                        FirstValueVisibility = Visibility.Hidden;
                        SecondValueVisibility = Visibility.Hidden;
                        break;
                    case 2:
                        if (checkRGB)
                        {
                            TitleText = "Operacje jednoargumentowe - Progowanie RGB";
                            ImgSourceAfter = Jednopunktowe.Progowanie(ImgSourceBefore, FirstValue);
                        }
                        if (checkHSV)
                        {
                            TitleText = "Operacje jednoargumentowe - Progowanie HSV";
                            ImgSourceAfter = Jednopunktowe.ProgowanieHSV(ImgSourceBefore, FirstValue);
                        }
                        SliderLabel = "Ustawienie p1";
                        FirstValueVisibility = Visibility.Visible;
                        SecondValueVisibility = Visibility.Hidden;
                        break;
                    case 3:
                        TitleText = "Operacje jednoargumentowe - Progowanie z zachowaniem poziomów szarości";
                        ImgSourceAfter = Jednopunktowe.ProgowanieZPoziomemSzarosci(ImgSourceBefore, FirstValue, SecondValue);
                        WyborKoloruGroupBoxVisibility = Visibility.Hidden;
                        //ImgSourceAfter = Jednopunktowe.Progowanie2(ImgSourceBefore, FirstValue, SecondValue);
                        SliderLabel = "Ustawienie p1 i p2";
                        FirstValueVisibility = Visibility.Visible;
                        SecondValueVisibility = Visibility.Visible;
                        break;
                    case 4:

                        if (checkRGB)
                        {
                            TitleText = "Operacje jednoargumentowe - Rozciąganie";
                            ImgSourceAfter = Jednopunktowe.RozciaganieRGB(ImgSourceBefore, FirstValue, SecondValue);
                        }
                        if (checkHSV)
                        {
                            TitleText = "Operacje jednoargumentowe - Rozciąganie HSV";
                            ImgSourceAfter = Jednopunktowe.RozciagnieHSV2(ImgSourceBefore, FirstValue, SecondValue);
                        }
                        SliderLabel = "Ustawienie p1 i p2";
                        FirstValueVisibility = Visibility.Visible;
                        SecondValueVisibility = Visibility.Visible;
                        break;
                    case 5:

                        if (checkRGB)
                        {
                            TitleText = "Operacje jednoargumentowe - Jasność RGB";
                            ImgSourceAfter = Jednopunktowe.Jasnosc(ImgSourceBefore, -255 + ((FirstValue) * 510) / 255);
                        }
                        if (checkHSV)
                        {
                            TitleText = "Operacje jednoargumentowe - Jasność HSV";
                            ImgSourceAfter = Jednopunktowe.Jasnosc_hsv(ImgSourceBefore, -255 + ((FirstValue) * 510) / 255);
                        }

                        SliderLabel = "Ustawienie p1";
                        FirstValueVisibility = Visibility.Visible;
                        SecondValueVisibility = Visibility.Hidden;
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
