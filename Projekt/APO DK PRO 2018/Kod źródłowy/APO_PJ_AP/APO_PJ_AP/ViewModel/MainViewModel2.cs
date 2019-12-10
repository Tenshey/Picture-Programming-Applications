using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using APO_PJ_AP.Model;
using APO_PJ_AP.Operation;
using APO_PJ_AP.View;
using GalaSoft.MvvmLight;
using OxyPlot;
using Application = System.Windows.Application;

namespace APO_PJ_AP.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel2 : ViewModelBase
    {
        //https://github.com/piecioshka/apo-photoshop

        #region ICommand
        public ICommand BrowseCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand ConvertToGrayscaleCommand { get; set; }
        public ICommand JednorgumentoweCommand { get; set; }

        public ICommand RGBiHSVCommand { get; set; }
        public ICommand ModelBarwCommand { get; set; }

        public ICommand CompareCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand WieloargumentoweCommand { get; set; }

        public ICommand HistogramWindowCommand { get; set; }
        public ICommand JednoargumentoweWindowCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ChangeBitmapCommand { get; set; }
        public ICommand SasiedztwaWindowCommand { get; set; }
        public ICommand OperacjeMorfologiczneWindowCommand { get; set; }
        #endregion

        #region Variables
        public ObservableCollection<MenuModel> MenuItems { get; set; }
        public ObservableCollection<Bitmap> BitmapCollection { get; set; }

        private int _ScalaValue;

        public int ScalaValue
        {
            get => _ScalaValue;
            set { Set(() => ScalaValue, ref _ScalaValue, value); }
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

        private Bitmap _imgSourceBefore;

        public Bitmap ImgSourceBefore
        {
            get => _imgSourceBefore;
            set
            {
                Set(() => ImgSourceBefore, ref _imgSourceBefore, value);
            }
        }

        private Bitmap _imgSourceAfter;
        public Bitmap ImgSourceAfter
        {
            get => _imgSourceAfter;
            set
            {
                Set(() => ImgSourceAfter, ref _imgSourceAfter, value);
            }
        }
        private String _pathToFile;
        public String PathToFile
        {
            get => _pathToFile;
            set { Set(() => PathToFile, ref _pathToFile, value); }
        }


        #endregion

        #region Constructor
        public MainViewModel2()
        {
#if DEBUG
           var cTukanApozHalowinBmp = @"C:\tukan\APOZ\halowin.bmp";
            if (File.Exists(cTukanApozHalowinBmp))
                ImgSourceBefore = new Bitmap(cTukanApozHalowinBmp);
            cTukanApozHalowinBmp = @"C:\Users\pjarocki\Documents\apoz\halowin.bmp";
            if (File.Exists(cTukanApozHalowinBmp))
                ImgSourceBefore = new Bitmap(cTukanApozHalowinBmp);
#endif
            BrowseCommand = new RelayCommand(ExecuteBrowseCommand, CanExecuteBrowseCommand);
            AboutCommand = new RelayCommand(ExecuteAboutCommand, CanExecuteBrowseCommand);
            ConvertToGrayscaleCommand = new RelayCommand(ExecuteConvertToGrayscaleCommand, CanExecuteConvertToGrayscaleCommand);
            SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CloseCommand = new RelayCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
            RGBiHSVCommand = new RelayCommand(ExecuteRGBiHSVCommand, CanExecuteBrowseCommand);
            ModelBarwCommand = new RelayCommand(ExecuteModelBarwCommand, CanExecuteBrowseCommand);
            HistogramWindowCommand = new RelayCommand(ExecuteHistogramWindowCommand, CanExecuteCommand);
            JednoargumentoweWindowCommand = new RelayCommand(ExecuteJednoargumentoweCommand, CanExecuteCommand);
            WieloargumentoweCommand = new RelayCommand(ExecuteWieloargumentoweCommand, CanExecuteCommand);
            SasiedztwaWindowCommand = new RelayCommand(ExecuteSasiedztwaWindowCommand, CanExecuteCommand);
            OperacjeMorfologiczneWindowCommand = new RelayCommand(ExecuteOperacjeMorfologiczneWindowCommand, CanExecuteCommand);

            //CompareCommand = new RelayCommand(ExecuteCompareCommand, CanExecuteCompareCommand);

            JednorgumentoweCommand = new RelayCommand(ExecuteJednopunktoweCommand, CanExecuteCommand);


            ChangeBitmapCommand = new RelayCommand(ExecuteChangeBitmapCommand, CanExecuteChangeBitmapCommand);
            BitmapCollection = new ObservableCollection<Bitmap>();

            MenuItems = new ObservableCollection<MenuModel>
                {
                    new MenuModel { Header = "Pliki",
                        MenuItems = new ObservableCollection<MenuModel>
                            {
                                new MenuModel { Header = "Otwórz", Command = BrowseCommand },
                                new MenuModel { Header = "Zapisz", Command = SaveCommand},
                                new MenuModel { Header = "Zamknij aplikacjê", Command = CloseCommand}
                            }
                    },

                    new MenuModel { Header = "Funkcje",
                        MenuItems = new ObservableCollection<MenuModel>
                            {
                                new MenuModel { Header = "Skala szaroœci", Command = ConvertToGrayscaleCommand },
                                new MenuModel { Header = "Histogram",
                                    MenuItems = new ObservableCollection<MenuModel>
                                    {
                                        new MenuModel { Header = "Wyg³adzanie",
                                            MenuItems = new ObservableCollection<MenuModel>
                                            {
                                                new MenuModel { Header = "Metoda œrednich",
                                                Index = 1,
                                                Command = HistogramWindowCommand
                                                },
                                                new MenuModel { Header = "Metoda losowych",
                                                Index = 2,
                                                Command = HistogramWindowCommand
                                                },
                                                new MenuModel { Header = "Metoda s¹siedztwa",
                                                Index = 3,
                                                Command = HistogramWindowCommand
                                                },
                                                new MenuModel { Header = "Metoda w³asna",
                                                Index = 4,
                                                Command = HistogramWindowCommand
                                                }
                                            }
                                        },
                                        new MenuModel { Header = "Rozci¹ganie",
                                        Index = 5,
                                        Command = HistogramWindowCommand
                                        }
                                    }
                                },
                                new MenuModel { Header = "Jednopunktowe",
                                    MenuItems = new ObservableCollection<MenuModel>
                                    {
                                        new MenuModel { Header = "Jednoargumentowe",                             MenuItems = new ObservableCollection<MenuModel>
                                        {
                                            new MenuModel { Header = "Negacja",
                                                Index = 1,
                                                Command = JednoargumentoweWindowCommand
                                            },
                                            new MenuModel { Header = "Progowanie",
                                                Index = 2,
                                                Command = JednoargumentoweWindowCommand
                                            },
                                            new MenuModel { Header = "Progowanie z zachowaniem poziomów szaroœci",
                                                Index = 3,
                                                Command = JednoargumentoweWindowCommand
                                            },
                                            new MenuModel { Header = "Rozci¹ganie",
                                                Index = 4,
                                                Command = JednoargumentoweWindowCommand
                                            },
                                            new MenuModel { Header = "Jasnoœæ",
                                                Index = 5,
                                                Command = JednoargumentoweWindowCommand
                                            }
                                        }},
                                        new MenuModel { Header = "Wieloargumentowe", Command = WieloargumentoweCommand }
                                    }
                                },
                                new MenuModel { Header = "S¹siedztwa",
                                    MenuItems = new ObservableCollection<MenuModel>
                                    {
                                        new MenuModel { Header = "Filtracja dolnoprzepustowa",
                                            MenuItems = new ObservableCollection<MenuModel>
                                            {
                                                new MenuModel { Header = " Filtracja liniowa", Command = SasiedztwaWindowCommand, Index = 1},
                                                new MenuModel { Header = " Filtracja medianowa", Command = SasiedztwaWindowCommand, Index = 2}
                                            }
                                        },
                                        new MenuModel { Header = "Filtracja górnoprzepustowa",
                                            MenuItems = new ObservableCollection<MenuModel>
                                            {
                                                new MenuModel { Header = " Filtracja gradientowa", Command = SasiedztwaWindowCommand, Index = 3},
                                                new MenuModel { Header = " Filtracja laplasjanowa", Command = SasiedztwaWindowCommand, Index = 4},
                                                new MenuModel { Header = " Metoda uzgadniania wzorca", Command = SasiedztwaWindowCommand, Index = 5}
                                            }
                                        }
                                    }
                                },
                                new MenuModel { Header = "Operacje morfologiczne",
                                    MenuItems = new ObservableCollection<MenuModel>
                                    {
                                        new MenuModel { Header = "Dylatacja",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 1,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 2,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Erozja",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 3,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 4,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Otwarcia",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 5,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 6,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    }
                                }
                                },
                                new MenuModel { Header = "Zamkniêcia",
                                MenuItems = new ObservableCollection<MenuModel>
                                {
                                    new MenuModel { Header = "Czterospojne",
                                    Index = 7,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    },
                                    new MenuModel { Header = "Osmiospojne",
                                    Index = 8,
                                    Command = OperacjeMorfologiczneWindowCommand
                                    }
                                }
                                }
                                    }


                                },
                                new MenuModel { Header = "Segmentacja",
                                    MenuItems = new ObservableCollection<MenuModel>
                                    {

                                new MenuModel { Header = "Progowanie",
                                                Index = 9,
                                                Command = OperacjeMorfologiczneWindowCommand
                                }
                                    }}
                            }
                    },
                    new MenuModel { Header = "Obrazy",
                        MenuItems = new ObservableCollection<MenuModel>(),
                        MenuItemVisibility = Visibility.Hidden
                    },
                    new MenuModel { Header = "Skale kolorów",
                        MenuItems = new ObservableCollection<MenuModel>
                        {
                            new MenuModel { Header = "RGB i HSV", Command = RGBiHSVCommand },
                            new MenuModel { Header = "Modele barw", Command = ModelBarwCommand}
                        }
                    },
                    new MenuModel { Header = "Pomoc", Command = AboutCommand}
                };

            ScalaValue = 10;

        }

        #endregion


        #region HistogramOptionsCommand
        public void ExecuteJednoargumentoweCommand(object parameter)
        {
            if (parameter != null)
            {
                int value = (int)parameter;
                var newWindow = new JednoargumentoweWindow(ImgSourceBefore, value);
                newWindow.ShowDialog();
                if (newWindow.jednoargViewModel.IsSaved)
                {
                    ImgSourceBefore = newWindow.jednoargViewModel.ImgSourceAfter;
                    AddBitmapToCollection((Bitmap)ImgSourceBefore.Clone(), generateNewNameForBitmap());

                }
            }
        }
        #endregion


        public void ExecuteSasiedztwaWindowCommand(object parameter)
        {
            if (parameter != null)
            {
                int value = (int)parameter;
                var newWindow = new SasiedztwaWindow(ImgSourceBefore, value);
                newWindow.ShowDialog();
                if (newWindow.sasiedztwaViewModel.IsSaved)
                {
                    ImgSourceBefore = newWindow.sasiedztwaViewModel.ImgSourceAfter;
                    AddBitmapToCollection((Bitmap)ImgSourceBefore.Clone(), generateNewNameForBitmap());
                }
            }
        }

        public void ExecuteHistogramWindowCommand(object parameter)
        {
            if (parameter != null)
            {
                int value = (int)parameter;
                var newWindow = new HistogramWindow(ImgSourceBefore, value);
                newWindow.ShowDialog();
                if (newWindow.histogramViewModel.IsSaved)
                {
                    ImgSourceBefore = newWindow.histogramViewModel.ImgSourceAfter;
                    AddBitmapToCollection((Bitmap)ImgSourceBefore.Clone(), generateNewNameForBitmap());
                }
            }
        }

        public void ExecuteOperacjeMorfologiczneWindowCommand(object parameter)
        {
            if (parameter != null)
            {
                int value = (int)parameter;
                var newWindow = new OperacjeMorfologiczneWindow(ImgSourceBefore, value);
                newWindow.ShowDialog();
                if (newWindow.operacjeMorfologiczneViewModel.IsSaved)
                {
                    ImgSourceBefore = newWindow.operacjeMorfologiczneViewModel.ImgSourceAfter;
                    AddBitmapToCollection((Bitmap)ImgSourceBefore.Clone(), generateNewNameForBitmap());
                }
            }
        }

        public void ExecuteSaveCommand(object parameter)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Zapisz plik";
            fd.Filter = "Image Files(*.BMP)|*.BMP";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                ImgSourceBefore.Save(fd.FileName);
            }
        }

        public bool CanExecuteSaveCommand(object parameter)
        {
            if (ImgSourceBefore != null)
                return true;
            return false;
        }

        #region BrowseCommand

        public void ExecuteAboutCommand(object parameter)
        {

            if (parameter != null)
            {
                int value = (int)parameter;
                var newWindow = new AboutWindow();
                newWindow.ShowDialog();

            }
        }

        public bool CanExecuteBrowseCommand(object parameter)
        {
            return true;
        }
        #endregion

        public void ExecuteBrowseCommand(object parameter)
        {

            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Wybierz plik";
            fd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                PathToFile = fd.FileName;
                ImgSourceBefore = new Bitmap(PathToFile);
                ImgSourceAfter = null;
                AddBitmapToCollection(ImgSourceBefore, Path.GetFileName(PathToFile));
            }
        }

        #region ConvertToGrayscaleCommand

        public void ExecuteConvertToGrayscaleCommand(object parameter)
        {

            using (new WaitCursor())
            {
                ImgSourceBefore = Jednopunktowe.ConvertToGrayscale((Bitmap)ImgSourceBefore.Clone());
                AddBitmapToCollection(ImgSourceBefore, generateNewNameForBitmap());
            }

        }

        public bool CanExecuteConvertToGrayscaleCommand(object parameter)
        {
            if (ImgSourceBefore != null)
                return true;
            return false;
        }
        #endregion

        #region CompareCommand

        public void ExecuteCompareCommand(object parameter)
        {
            //var newWindow = new CompareView(RedPointCollectionBefore, RedPointCollectionAfter);
            //newWindow.ShowDialog();
        }

        public bool CanExecuteCompareCommand(object parameter)
        {
            if (ImgSourceBefore == null || ImgSourceAfter == null)
                return false;
            return true;
        }
        #endregion

        #region CloseCommand

        public void ExecuteCloseCommand(object parameter)
        {
            Application.Current.Shutdown();
        }
        public void ExecuteRGBiHSVCommand(object parameter)
        {
            SkalaHSV obrazSAS = new SkalaHSV();
            //  obrazSAS.fileloc = oFileDlg.FileName;
            obrazSAS.ShowDialog();
        }
        public void ExecuteModelBarwCommand(object parameter)
        {
            SkalaRGB obrazS = new SkalaRGB();
            //  obrazSAS.fileloc = oFileDlg.FileName;
            obrazS.ShowDialog();
        }

        public bool CanExecuteCloseCommand(object parameter)
        {
            return true;
        }


        #endregion

        #region WieloargumentoweCommand

        public void ExecuteWieloargumentoweCommand(object parameter)
        {
            var newWindow = new WieloargumentoweWindow(ImgSourceBefore);
            newWindow.ShowDialog();
            if (newWindow.WieloargViewModel.ImgSourceAfter != null) ImgSourceAfter = newWindow.WieloargViewModel.ImgSourceAfter;
        }


        #endregion

        #region JednopunktoweCommand

        #region WieloargumentoweCommand

        public void ExecuteJednopunktoweCommand(object parameter)
        {
            Window newWindow = new WieloargumentoweWindow(ImgSourceBefore);
            newWindow.Show();
        }

        public bool CanExecuteCommand(object parameter)
        {
            if (ImgSourceBefore == null)
                return false;

            return true;
        }
        #endregion

        #endregion

        #region ChangeBitmapCommand
        public void ExecuteChangeBitmapCommand(object parameter)
        {
            if (parameter != null)
            {
                int index = (int)parameter;
                if (MenuItems[2].MenuItems.Count > 1)
                {
                    foreach (MenuModel menuItem in MenuItems[2].MenuItems)
                        if (menuItem.Index != index)
                            menuItem.IsChecked = false;
                        else
                        {
                            menuItem.IsChecked = true;
                            ImgSourceBefore = BitmapCollection[index];
                        }
                }
            }
        }

        public bool CanExecuteChangeBitmapCommand(object parameter)
        {
            return true;
        }
        #endregion



        public void AddBitmapToCollection(Bitmap bmp, string name)
        {
            BitmapCollection.Add(bmp);
            if (MenuItems[2].MenuItems.Count > 0)
            {
                foreach (MenuModel menuItem in MenuItems[2].MenuItems)
                    menuItem.IsChecked = false;
            }
            else
                MenuItems[2].MenuItemVisibility = Visibility.Visible;
            MenuItems[2].MenuItems.Add(new MenuModel
            {
                Index = MenuItems[2].MenuItems.Count,
                Command = ChangeBitmapCommand,
                Header = name,
                IsChecked = true
            });
        }

        public string generateNewNameForBitmap()
        {
            string name = "Obraz wynikowy";
            string name2 = name;
            int counter = 0;
            bool result = false;
            if (MenuItems[2].MenuItems.Count > 0)
            {
                do
                {
                    if (MenuItems[2].MenuItems.Any(x => x.Header.Equals(name)))
                        name2 = name + "_" + ++counter;
                } while (result);
            }
            return name2;
        }
    }
}