using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using APO_PJ_AP.Model;
using APO_PJ_AP.Operation;
using GalaSoft.MvvmLight;

namespace APO_PJ_AP.ViewModel
{
    public class SasiedztwaViewModel : ViewModelBase
    {
        #region ICommand
        public ICommand MethodCommand { get; set; }
        public ICommand DoMethodCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region Variables
        public ObservableCollection<MenuModel> MenuItems { get; set; }
        private ObservableCollection<KeyValuePair<string, MaskClass>> _maskTemplates = new ObservableCollection<KeyValuePair<string, MaskClass>>();
        public ObservableCollection<KeyValuePair<string, MaskClass>> MaskTemplates
        {
            get => _maskTemplates;
            set
            {
                Set(() => MaskTemplates, ref _maskTemplates, value);
            }
        }
        private ObservableCollection<KeyValuePair<string, MaskClass>> _kierunek = new ObservableCollection<KeyValuePair<string, MaskClass>>();
        public ObservableCollection<KeyValuePair<string, MaskClass>> Kierunek
        {
            get => _kierunek;
            set
            {
                Set(() => Kierunek, ref _kierunek, value);
            }
        }
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

        private DataView _maskDataView;
        public DataView MaskDataView
        {
            get => _maskDataView;
            set
            {
                Set(() => MaskDataView, ref _maskDataView, value);
            }
        }
        private DataView _maskDataView2;
        public DataView MaskDataView2
        {
            get => _maskDataView2;
            set
            {
                Set(() => MaskDataView2, ref _maskDataView2, value);
            }
        }
        private MaskClass _maskValues;
        public MaskClass MaskValues
        {
            get => _maskValues;
            set
            {
                Set(() => MaskValues, ref _maskValues, value);
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

        private int _radioButtonValue;
        public int RadioButtonValue // przy oznaczneniu w combobox najpierw ustawia poprawną wartość dla radiobuttonvalue, ale przy ustawianiu wchodzi też do convertback i pobiera wartość 4 (później to ustawia i jest źle zaznaczone)
        {
            get => _radioButtonValue;
            set
            {
                Set(() => RadioButtonValue, ref _radioButtonValue, value);
            }
        }

        private MaskClass _selectedItem;
        public MaskClass SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(() => SelectedItem, ref _selectedItem, value);
            }
        }
        private MaskClass _selectedItem2;
        public MaskClass SelectedItem2
        {
            get => _selectedItem2;
            set
            {
                Set(() => SelectedItem2, ref _selectedItem2, value);
                getDataTable(value);
                FillWindow(WhichMethod);
            }
        }
        public int SelectedMethod { get; set; }
        private bool _isEnabled;
        public bool IsEnabled   // poprawne wartości pobiera, ale nie blokuje edycji
        {
            get => _isEnabled;
            set
            {
                Set(() => IsEnabled, ref _isEnabled, value);
            }
        }
        private Visibility _scaleMethodsVisibility;
        public Visibility ScaleMethodsVisibility  // poprawne wartości pobiera, ale nie blokuje edycji
        {
            get => _scaleMethodsVisibility;
            set
            {
                Set(() => ScaleMethodsVisibility, ref _scaleMethodsVisibility, value);
            }
        }
        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                Set(() => SelectedIndex, ref _selectedIndex, value);
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
        private Visibility _firstComboBoxVisibility;
        public Visibility FirstComboBoxVisibility
        {
            get => _firstComboBoxVisibility;
            set
            {
                Set(() => FirstComboBoxVisibility, ref _firstComboBoxVisibility, value);
            }
        }
        private Visibility _secondComboBoxVisibility;
        public Visibility SecondComboBoxVisibility
        {
            get => _secondComboBoxVisibility;
            set
            {
                Set(() => SecondComboBoxVisibility, ref _secondComboBoxVisibility, value);
            }
        }
        private bool? _chkRgb;
        public bool? ChkRGB
        {
            get { return _chkRgb; }
            set { Set(() => ChkRGB, ref _chkRgb, value); }
        }
        private bool? _chkMetoda1;
        public bool? ChkMetoda1
        {
            get { return _chkMetoda1; }
            set { Set(() => ChkMetoda1, ref _chkMetoda1, value); }
        }

        private bool? _chkMetoda2;
        public bool? ChkMetoda2
        {
            get { return _chkMetoda2; }
            set { Set(() => ChkMetoda2, ref _chkMetoda2, value); }
        }
        private bool? _chkMetoda3;
        public bool? ChkMetoda3
        {
            get { return _chkMetoda3; }
            set { Set(() => ChkMetoda3, ref _chkMetoda3, value); }
        }
        private bool? _chkHSV;
        public bool? ChkHSV
        {
            get { return _chkHSV; }
            set { Set(() => ChkHSV, ref _chkHSV, value); }
        }
        private Visibility _firstMask;
        public Visibility FirstMask
        {
            get => _firstMask;
            set
            {
                Set(() => FirstMask, ref _firstMask, value);
            }
        }
        private Visibility _secondMask;
        public Visibility SecondMask
        {
            get => _secondMask;
            set
            {
                Set(() => SecondMask, ref _secondMask, value);
            }
        }
        private Visibility _maskGroupVisibility;
        public Visibility MaskGroupVisibility
        {
            get => _maskGroupVisibility;
            set
            {
                Set(() => MaskGroupVisibility, ref _maskGroupVisibility, value);
            }
        }
        #endregion

        #region Constructor
        public SasiedztwaViewModel(Bitmap bmp, int value)
        {
            MethodCommand = new RelayCommand(ExecuteMethodCommand, CanExecuteMethodCommand);
            SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
            ImgSourceBefore = bmp;
            RadioButtonValue = 1;
            ChkRGB = true;
            ChkHSV = false;
            ChkMetoda1 = false;
            ChkMetoda2 = false;
            ChkMetoda3 = true;

            switch (value)
            {
                case 1:
                    fillMaskTemplatesFiltracjaLiniowa();
                    break;
                case 2:
                    fillMaskTemplatesFiltracjaMedianowa();
                    break;
                case 3:
                    fillMaskTemplatesFiltracjaGradientowa();
                    break;
                case 4:
                    fillMaskTemplatesFiltracjaLaplasjanowa();
                    break;
                case 5:
                    fillMaskTemplatesFiltracjaGradientowa();
                    break;
            }

            SelectedIndex = -1;
            FillWindow(value);
        }
        #endregion

        public void FillWindow(int value)
        {
            using (new WaitCursor())
            {
                bool checkRGB = ChkRGB != null && ChkRGB.Value;
                bool checkHSV = ChkHSV != null && ChkHSV.Value;

                int metoda = 1;

                if (ChkMetoda1 != null && ChkMetoda1.Value)
                    metoda = 1;
                if (ChkMetoda2 != null && ChkMetoda2.Value)
                    metoda = 2;
                if (ChkMetoda3 != null && ChkMetoda3.Value)
                    metoda = 3;

                WhichMethod = value;
                switch (value)
                {
                    case 1:
                        FirstComboBoxVisibility = Visibility.Visible;
                        SecondComboBoxVisibility = Visibility.Hidden;
                        FirstMask = Visibility.Visible;
                        SecondMask = Visibility.Hidden;
                        MaskGroupVisibility = Visibility.Hidden;
                        ScaleMethodsVisibility = Visibility.Hidden;
                        
                        if (checkRGB)
                        {
                            TitleText = "Filtracja liniowa - RGB";
                        }
                        if (checkHSV)
                        {
                            TitleText = "Filtracja liniowa - HSV";
                        }
                        if (MaskValues?.Values != null)
                            ImgSourceAfter = ImgSourceBefore.ConvolutionFilter(MaskValues.Values, true, isHsv: checkHSV);
                        break;
                    case 2:
                        FirstComboBoxVisibility = Visibility.Hidden;
                        SecondComboBoxVisibility = Visibility.Hidden;
                        FirstMask = Visibility.Hidden;
                        SecondMask = Visibility.Hidden;
                        MaskGroupVisibility = Visibility.Visible;
                        ScaleMethodsVisibility = Visibility.Hidden;
                        if (checkRGB)
                        {
                            TitleText = "Filtracja medianowa - RGB";
                        }
                        if (checkHSV)
                        {
                            TitleText = "Filtracja medianowa - HSV";
                        }
                        if (RadioButtonValue == 1)
                            ImgSourceAfter = ImgSourceBefore.MedianFilter(3);
                        else if (RadioButtonValue == 4)
                            ImgSourceAfter = ImgSourceBefore.MedianFilter(5);
                        else if (RadioButtonValue == 5)
                            ImgSourceAfter = ImgSourceBefore.MedianFilter(7);
                        break;
                    case 3:
                        FirstComboBoxVisibility = Visibility.Visible;
                        SecondComboBoxVisibility = Visibility.Hidden;
                        FirstMask = Visibility.Visible;
                        SecondMask = Visibility.Visible;
                        MaskGroupVisibility = Visibility.Hidden;
                        ScaleMethodsVisibility = Visibility.Visible;
                        if (checkRGB)
                        {
                            TitleText = "Filtracja gradientowa - RGB";
                        }
                        if (checkHSV)
                        {
                            TitleText = "Filtracja gradientowa - HSV";
                        }
                        if (MaskValues?.Values != null && MaskValues?.Values2 != null)
                            ImgSourceAfter = ImgSourceBefore.ConvolutionFilter(MaskValues.Values, MaskValues.Values2, 1, 0, false, checkHSV, metoda);
                        break;
                    case 4:
                        FirstComboBoxVisibility = Visibility.Visible;
                        SecondComboBoxVisibility = Visibility.Hidden;
                        FirstMask = Visibility.Visible;
                        SecondMask = Visibility.Hidden;
                        MaskGroupVisibility = Visibility.Hidden;
                        ScaleMethodsVisibility = Visibility.Visible;
                        if (checkRGB)
                        {
                            TitleText = "Filtracja laplasjanowa - RGB";
                        }
                        if (checkHSV)
                        {
                            TitleText = "Filtracja laplasjanowa - HSV";
                        }
                        if (MaskValues?.Values != null)
                            ImgSourceAfter = ImgSourceBefore.ConvolutionFilter(MaskValues.Values, false, checkHSV, metoda);
                        break;
                    case 5:
                        FirstComboBoxVisibility = Visibility.Visible;
                        SecondComboBoxVisibility = Visibility.Visible;
                        FirstMask = Visibility.Visible;
                        SecondMask = Visibility.Hidden;
                        MaskGroupVisibility = Visibility.Hidden;
                        ScaleMethodsVisibility = Visibility.Visible;
                        if (checkRGB)
                        {
                            TitleText = "Metoda uzgadniania wzorca - RGB";
                        }
                        if (checkHSV)
                        {
                            TitleText = "Metoda uzgadniania wzorca - HSV";
                        }
                        if (MaskValues?.Values != null)
                            ImgSourceAfter = ImgSourceBefore.ConvolutionFilter(MaskValues.Values, false, checkHSV, metoda);
                        break;
                }
            }
        }

        public void ExecuteMethodCommand(object parameter)
        {
            FillWindow(WhichMethod);

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

        public bool CanExecuteDoMethodCommand(object parameter)
        {
            if (WhichMethod == 0)
                return false;
            return true;
        }

        #region Filtracja liniowa - maski
        public void fillMaskTemplatesFiltracjaLiniowa()
        {
            MaskTemplates = new ObservableCollection<KeyValuePair<string, MaskClass>>();
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Użytkownika", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, 1, 1 },
                    { 1, 1, 1 },
                    { 1, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Filtr uśredniający", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, 1, 1 },
                    { 1, 1, 1 },
                    { 1, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Filtr kwadratowy", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Filtr kołowy", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 0, 1, 1, 1, 0 },
                    { 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 1, 1 },
                    { 0, 1, 1, 1, 0 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("LP1", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, 1, 1 },
                    { 1, 2, 1 },
                    { 1, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("LP2", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, 1, 1 },
                    { 1, 4, 1 },
                    { 1, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("LP3", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, 1, 1 },
                    { 1, 12, 1 },
                    { 1, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Piramidalny", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 1, 2, 3, 2, 1 },
                    { 2, 4, 6, 4, 2 },
                    { 3, 6, 9, 6, 3 },
                    { 2, 4, 6, 4, 2 },
                    { 1, 2, 3, 2, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Stożkowy", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 0, 0, 1, 0, 0 },
                    { 0, 2, 2, 2, 0 },
                    { 1, 2, 5, 2, 1 },
                    { 0, 2, 2, 2, 0 },
                    { 0, 0, 1, 0, 0 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Gauss 1", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, 2, 1 },
                    { 2, 4, 2 },
                    { 1, 2, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Gauss 2", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 1, 1, 2, 1, 1 },
                    { 1, 2, 4, 2, 1 },
                    { 2, 4, 8, 4, 2 },
                    { 1, 2, 4, 2, 1 },
                    { 1, 1, 2, 1, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Gauss 3", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 0, 1, 2, 1, 0 },
                    { 1, 4, 8, 4, 1 },
                    { 2, 8, 16, 8, 2 },
                    { 1, 4, 8, 4, 1 },
                    { 0, 1, 2, 1, 0 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Gauss 4", new MaskClass
            {
                columnCount = 5,
                rowCount = 5,
                Values = new[,]
                {
                    { 1, 4, 7, 4, 1 },
                    { 4, 16, 26, 16, 4 },
                    { 7, 26, 41, 26, 7 },
                    { 4, 16, 26, 16, 4 },
                    { 1, 4, 7, 4, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Gauss 5", new MaskClass
            {
                columnCount = 7,
                rowCount = 7,
                Values = new[,]
                {
                    { 1, 1, 2, 2, 2, 1, 1 },
                    { 1, 2, 2, 4, 2, 2, 1 },
                    { 2, 2, 4, 8, 4, 2, 2 },
                    { 2, 4, 8, 16, 8, 4, 2 },
                    { 2, 2, 4, 8, 4, 2, 2 },
                    { 1, 2, 2, 4, 2, 2, 1 },
                    { 1, 1, 2, 2, 2, 1, 1 }
                }
            }));
        }
        #endregion
        #region Filtracja medianowa - maski
        public void fillMaskTemplatesFiltracjaMedianowa()
        {
            MaskTemplates = new ObservableCollection<KeyValuePair<string, MaskClass>>();
        }
        #endregion
        #region Filtracja gradientowa - maski
        public void fillMaskTemplatesFiltracjaGradientowa()
        {
            MaskTemplates = new ObservableCollection<KeyValuePair<string, MaskClass>>();
            //MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Robertsa", new MaskClass
            //{
            //    columnCount = 2,
            //    rowCount = 2,
            //    Values = new int[,]
            //    {
            //        { -1, 0 },
            //        { 0, 1 }
            //    },
            //    Values2 = new int[,]
            //    {
            //        { 0, 1 },
            //        { -1, 0 }
            //    }
            //}));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Prewitta", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { -1, -1, -1 },
                    { 0, 0, 0 },
                    { 1, 1, 1 }
                },
                Values2 = new[,]
                {
                    { -1, 0, 1 },
                    { -1, 0, 1 },
                    { -1, 0, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Sobela", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { -1, 0, 1 },
                    { -2, 0, 2 },
                    { -1, 0, 1 }
                },
                Values2 = new[,]
                {
                    { 1, 2, 1 },
                    { 0, 0, 1 },
                    { -1, -2, -1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Kirscha", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 5, 5, 5 },
                    { -3, 0, -3 },
                    { -3, -3, -3 }
                },
                Values2 = new[,]
                {
                    { 5, -3, -3 },
                    { 5, 0, -3 },
                    { 5, -3, -3 }
                }
            }));
        }
        #endregion
        #region Filtracja laplasjanowa - maski
        public void fillMaskTemplatesFiltracjaLaplasjanowa()
        {
            MaskTemplates = new ObservableCollection<KeyValuePair<string, MaskClass>>();
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("LAPL1", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 0, -1, 0 },
                    { -1, 4, -1 },
                    { 0, -1, 0 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("LAPL2", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { -1, -1, -1 },
                    { -1, 8, -1 },
                    { -1, -1, -1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("LAPL3", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 1, -2, 1 },
                    { -2, 4, -2 },
                    { 1, -2, 1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Laplace'a ukośny", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { -1, 0, -1 },
                    { 0, 4, 0 },
                    { -1, 0, -1 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Laplace'a poziomy (krawędzie poziome)", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 0, -1, 0 },
                    { 0, 2, 0 },
                    { 0, -1, 0 }
                }
            }));
            MaskTemplates.Add(new KeyValuePair<string, MaskClass>("Laplace'a poziomy (krawędzie pionowe)", new MaskClass
            {
                columnCount = 3,
                rowCount = 3,
                Values = new[,]
                {
                    { 0, 0, 0 },
                    { -1, 2, -1 },
                    { 0, 0, 0 }
                }
            }));
        }
        #endregion
        #region Filtracja metoda uzgadniania wzorca - maski
        public void fillMaskTemplatesMetodaUzgadnianiaWzorca()
        {
            Kierunek = new ObservableCollection<KeyValuePair<string, MaskClass>>();
            switch (SelectedIndex)
            {
                case 0:
                    #region Prewitt
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północ", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, 1 },
                            { 1, -2, 1 },
                            { -1, -1, -1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północny - wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, 1 },
                            { -1, -2, 1 },
                            { -1, -1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -1, 1, 1 },
                            { -1, -2, 1 },
                            { -1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południowy - wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -1, -1, 1 },
                            { -1, -2, 1 },
                            { 1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południe", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -1, -1, -1 },
                            { 1, -2, 1 },
                            { 1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południowy - zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, -1, -1 },
                            { 1, -2, -1 },
                            { 1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, -1 },
                            { 1, -2, -1 },
                            { 1, 1, -1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północny - zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, 1 },
                            { 1, -2, -1 },
                            { 1, -1, -1 }
                        }
                    }));
                    #endregion
                    break;
                case 1:
                    #region Sobel
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północ", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, 1 },
                            { 1, -2, 1 },
                            { -1, -1, -1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północny - wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, 1 },
                            { -1, -2, 1 },
                            { -1, -1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -1, 1, 1 },
                            { -1, -2, 1 },
                            { -1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południowy - wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -1, -1, 1 },
                            { -1, -2, 1 },
                            { 1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południe", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -1, -1, -1 },
                            { 1, -2, 1 },
                            { 1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południowy - zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, -1, -1 },
                            { 1, -2, -1 },
                            { 1, 1, 1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, -1 },
                            { 1, -2, -1 },
                            { 1, 1, -1 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północny - zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 1, 1, 1 },
                            { 1, -2, -1 },
                            { 1, -1, -1 }
                        }
                    }));
                    #endregion
                    break;
                case 2:
                    #region Kirsch
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północ", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 3, 3, 3 },
                            { 3, 0, 3 },
                            { -5, -5, -5 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północny - wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 3, 3, 3 },
                            { -5, 0, 3 },
                            { -5, -5, 3 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -5, 3, 3 },
                            { -5, 0, 3 },
                            { -5, 3, 3 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południowy - wschód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -5, -5, 3 },
                            { -5, 0, 3 },
                            { 3, 3, 3 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południe", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { -5, -5, -5 },
                            { 3, 0, 3 },
                            { 3, 3, 3 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Południowy - zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 3, -5, -5 },
                            { 3, 0, -5 },
                            { 3, 3, 3 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 3, 3, -5 },
                            { 3, 0, -5 },
                            { 3, 3, -5 }
                        }
                    }));
                    Kierunek.Add(new KeyValuePair<string, MaskClass>("Północny - zachód", new MaskClass
                    {
                        columnCount = 3,
                        rowCount = 3,
                        Values = new[,]
                        {
                            { 3, 3, 3 },
                            { 3, 0, -5 },
                            { 3, -5, -5 }
                        }
                    }));
                    #endregion
                    break;
            }
        }
        #endregion

        public void getDataTable(MaskClass MaskValues = null)
        {
            if (MaskValues == null) // nowa maska na podstawie zaznaczenia radiobuttona
            {
                MaskValues = new MaskClass(RadioButtonValue);
                //SelectedIndex = -1; // zmiana comboboxa na element -1 (pusty)
                IsEnabled = true;   // możliwość edytowania datagrid
            }
            else // maska na podstawie wyboru z comboboxa
            {
                IsEnabled = false;  // zablokowane edytowanie datagrid
                if (MaskValues.columnCount == 7)    //zaznaczenie na radiobutton wartości odpowiadającej masce
                    RadioButtonValue = 5;
                else if (MaskValues.columnCount == 5)
                {
                    if (MaskValues.rowCount == 5)
                        RadioButtonValue = 4;
                    else if (MaskValues.rowCount == 3)
                        RadioButtonValue = 3;
                }
                else if (MaskValues.rowCount == 3)
                {
                    if (MaskValues.columnCount == 5)
                        RadioButtonValue = 2;
                    else if (MaskValues.columnCount == 3)
                        RadioButtonValue = 1;
                }
            }

            this.MaskValues = MaskValues;
            DataTable dataTable = new DataTable();

            for (var c = 0; c < MaskValues.columnCount; c++)
            {
                dataTable.Columns.Add(new DataColumn(c.ToString(), typeof(int)));
            }

            for (var r = 0; r < MaskValues.rowCount; r++)
            {
                DataRow newRow = dataTable.NewRow();

                newRow[0] = MaskValues.Values[r, 0];
                newRow[1] = MaskValues.Values[r, 1];

                if (MaskValues.columnCount == 3 || MaskValues.columnCount == 5 || MaskValues.columnCount == 7)
                {
                    newRow[2] = MaskValues.Values[r, 2];
                    if (MaskValues.columnCount == 5 || MaskValues.columnCount == 7)
                    {
                        newRow[3] = MaskValues.Values[r, 3];
                        newRow[4] = MaskValues.Values[r, 4];
                        if (MaskValues.columnCount == 7)
                        {
                            newRow[5] = MaskValues.Values[r, 5];
                            newRow[6] = MaskValues.Values[r, 6];
                        }
                    }
                }

                dataTable.Rows.Add(newRow);
            }

            MaskDataView = dataTable.DefaultView;
        }

        public void getDataTable2(MaskClass MaskValues)
        {
            if (MaskValues == null)
            {
                MaskDataView2 = null;
            }
            else
            {
                DataTable dataTable = new DataTable();

                for (var c = 0; c < MaskValues.columnCount; c++)
                {
                    dataTable.Columns.Add(new DataColumn(c.ToString(), typeof(int)));
                }

                for (var r = 0; r < MaskValues.rowCount; r++)
                {
                    DataRow newRow = dataTable.NewRow();

                    newRow[0] = MaskValues.Values2[r, 0];
                    newRow[1] = MaskValues.Values2[r, 1];

                    if (MaskValues.columnCount == 3 || MaskValues.columnCount == 5 || MaskValues.columnCount == 7)
                    {
                        newRow[2] = MaskValues.Values2[r, 2];
                        if (MaskValues.columnCount == 5 || MaskValues.columnCount == 7)
                        {
                            newRow[3] = MaskValues.Values2[r, 3];
                            newRow[4] = MaskValues.Values2[r, 4];
                            if (MaskValues.columnCount == 7)
                            {
                                newRow[5] = MaskValues.Values2[r, 5];
                                newRow[6] = MaskValues.Values2[r, 6];
                            }
                        }
                    }

                    dataTable.Rows.Add(newRow);
                }

                MaskDataView2 = dataTable.DefaultView;
            }
        }
    }
}
