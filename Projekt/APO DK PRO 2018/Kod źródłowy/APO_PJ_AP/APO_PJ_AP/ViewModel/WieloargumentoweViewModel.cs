using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using APO_PJ_AP.Model;
using APO_PJ_AP.Operation;
using Microsoft.Win32;

namespace APO_PJ_AP.ViewModel
{
    public class WieloargumentoweViewModel : MainViewModel2
    {
        #region Variables

        private Bitmap _firstImageBmp;

        public Bitmap FirstImageBmp
        {
            get => _firstImageBmp;
            set { Set(() => FirstImageBmp, ref _firstImageBmp, value); }
        }


        private Bitmap _secondImageBmp;

        public Bitmap SecondImageBmp
        {
            get => _secondImageBmp;
            set { Set(() => SecondImageBmp, ref _secondImageBmp, value); }
        }


        private Bitmap _resultImageBmp;

        public Bitmap ResultImageBmp
        {
            get => _resultImageBmp;
            set { Set(() => ResultImageBmp, ref _resultImageBmp, value); }
        }

        private Visibility _redPolygonVisibility = Visibility.Visible;

        public Visibility RedPolygonVisibility
        {
            get => _redPolygonVisibility;
            set { Set(() => RedPolygonVisibility, ref _redPolygonVisibility, value); }
        }

        private Visibility _greenPolygonVisibility = Visibility.Visible;

        public Visibility GreenPolygonVisibility
        {
            get => _greenPolygonVisibility;
            set { Set(() => GreenPolygonVisibility, ref _greenPolygonVisibility, value); }
        }

        private Visibility _bluePolygonVisibility = Visibility.Visible;

        public Visibility BluePolygonVisibility
        {
            get => _bluePolygonVisibility;
            set { Set(() => BluePolygonVisibility, ref _bluePolygonVisibility, value); }
        }

        private bool _redCheckBox = true;

        public bool RedCheckBox
        {
            get => _redCheckBox;
            set
            {
                RedPolygonVisibility = value ? Visibility.Visible : Visibility.Hidden;
                Set(() => RedCheckBox, ref _redCheckBox, value);
            }
        }

        private bool _greenCheckBox = true;

        public bool GreenCheckBox
        {
            get => _greenCheckBox;
            set
            {
                GreenPolygonVisibility = value ? Visibility.Visible : Visibility.Hidden;
                Set(() => GreenCheckBox, ref _greenCheckBox, value);
            }
        }

        private bool _blueCheckBox = true;

        public bool BlueCheckBox
        {
            get => _blueCheckBox;
            set
            {
                BluePolygonVisibility = value ? Visibility.Visible : Visibility.Hidden;
                Set(() => BlueCheckBox, ref _blueCheckBox, value);
            }
        }

        private PointCollection _redPointCollection;
        public PointCollection RedPointCollection
        {
            get => _redPointCollection;
            set { Set(() => RedPointCollection, ref _redPointCollection, value); }
        }

        private PointCollection _greenPointCollection;
        public PointCollection GreenPointCollection
        {
            get => _greenPointCollection;
            set { Set(() => GreenPointCollection, ref _greenPointCollection, value); }
        }

        private PointCollection _bluePointCollection;
        public PointCollection BluePointCollection
        {
            get => _bluePointCollection;
            set { Set(() => BluePointCollection, ref _bluePointCollection, value); }
        }

        private Visibility _checkBoxVisibility = Visibility.Hidden;

        public Visibility CheckBoxVisibility
        {
            get => _checkBoxVisibility;
            set { Set(() => CheckBoxVisibility, ref _checkBoxVisibility, value); }
        }

        private Visibility _secondImageTabControlVisibility = Visibility.Hidden;

        public Visibility SecondImageTabControlVisibility
        {
            get => _secondImageTabControlVisibility;
            set { Set(() => SecondImageTabControlVisibility, ref _secondImageTabControlVisibility, value); }
        }

        private Visibility _resultImageTabControlVisibility = Visibility.Hidden;

        public Visibility ResultImageTabControlVisibility
        {
            get => _resultImageTabControlVisibility;
            set { Set(() => ResultImageTabControlVisibility, ref _resultImageTabControlVisibility, value); }
        }
        #endregion

        #region ICommand

        public ICommand CopyImageCommand { get; set; }
        public ICommand OpenImageCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SubCommand { get; set; }
        public ICommand DifferenceCommand { get; set; }
        public ICommand AndCommand { get; set; }
        public ICommand OrCommand { get; set; }
        public ICommand XorCommand { get; set; }
        #endregion

        public WieloargumentoweViewModel()
        {

            CopyImageCommand = new RelayCommand(ExecuteCopyImageCommand, CanExecuteCopyImageCommand);
            OpenImageCommand = new RelayCommand(ExecuteOpenImageCommand, CanExecuteOpenImageCommand);
            BrowseCommand = new RelayCommand(ExecuteBrowseCommand, CanExecuteBrowseCommand);
            AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteResultCommand);
            SubCommand = new RelayCommand(ExecuteSubCommand, CanExecuteResultCommand);
            DifferenceCommand = new RelayCommand(ExecuteDifferenceCommand, CanExecuteResultCommand);
            AndCommand = new RelayCommand(ExecuteAndCommand, CanExecuteResultCommand);
            OrCommand = new RelayCommand(ExecuteOrCommand, CanExecuteResultCommand);
            XorCommand = new RelayCommand(ExecuteXorCommand, CanExecuteResultCommand);
        }

        #region OpenImageCommand

        public void ExecuteOpenImageCommand(object parameter)
        {

            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Wybierz plik";
            fd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";

            if (fd.ShowDialog() == true)
            {
                PathToFile = fd.FileName;
                SecondImageBmp = new Bitmap(PathToFile);
                SecondImageTabControlVisibility = Visibility.Visible;
            }
        }

        public bool CanExecuteOpenImageCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region CopyImageCommand
        public void ExecuteCopyImageCommand(object parameter)
        {
            SecondImageBmp = FirstImageBmp;
            SecondImageTabControlVisibility = Visibility.Visible;
        }

        public bool CanExecuteCopyImageCommand(object parameter)
        {
            return true;
        }
        #endregion

        public bool CanExecuteResultCommand(object parameter)
        {
            if (FirstImageBmp == null || SecondImageBmp == null)
                return false;
            return true;
        }

        public void ExecuteAddCommand(object parameter)
        {
            using (new WaitCursor())
            {
                Bitmap newBmp = Jednopunktowe.AddMethod(FirstImageBmp, SecondImageBmp);
                ResultImageBmp = newBmp;

                ResultImageTabControlVisibility = Visibility.Visible;
            }
        }

        public void ExecuteSubCommand(object parameter)
        {
            using (new WaitCursor())
            {
                Bitmap newBmp = Jednopunktowe.SubMethod(FirstImageBmp, SecondImageBmp);
                ResultImageBmp = newBmp;

                ResultImageTabControlVisibility = Visibility.Visible;
            }
        }

        public void ExecuteDifferenceCommand(object parameter)
        {
            using (new WaitCursor())
            {
                Bitmap newBmp = Jednopunktowe.DifferenceMethod(FirstImageBmp, SecondImageBmp);
                ResultImageBmp = newBmp;

                ResultImageTabControlVisibility = Visibility.Visible;
            }
        }

        public void ExecuteAndCommand(object parameter)
        {
            using (new WaitCursor())
            {
                Bitmap newBmp = Jednopunktowe.AndMethod(FirstImageBmp, SecondImageBmp);
                ResultImageBmp = newBmp;

                ResultImageTabControlVisibility = Visibility.Visible;

            }
        }

        public void ExecuteOrCommand(object parameter)
        {
            using (new WaitCursor())
            {
                Bitmap newBmp = Jednopunktowe.OrMethod(FirstImageBmp, SecondImageBmp);
                ResultImageBmp = newBmp;

                ResultImageTabControlVisibility = Visibility.Visible;
            }
        }

        public void ExecuteXorCommand(object parameter)
        {
            using (new WaitCursor())
            {
                Bitmap newBmp = Jednopunktowe.XorMethod(FirstImageBmp, SecondImageBmp);
                ResultImageBmp = newBmp;
                ResultImageTabControlVisibility = Visibility.Visible;
            }

        }
    }
}
