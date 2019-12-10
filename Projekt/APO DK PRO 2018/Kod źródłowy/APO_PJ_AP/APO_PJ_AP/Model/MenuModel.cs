using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace APO_PJ_AP.Model
{
    public class MenuModel : ViewModelBase
    {
        public string Header { get; set; }
        public ObservableCollection<MenuModel> MenuItems { get; set; }
        public ICommand Command { get; set; }
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                Set(() => IsChecked, ref _isChecked, value);
            }
        }
        private Visibility _menuItemVisibility;
        public Visibility MenuItemVisibility
        {
            get => _menuItemVisibility;
            set
            {
                Set(() => MenuItemVisibility, ref _menuItemVisibility, value);
            }
        }
        public int Index { get; set; }
    }
}
