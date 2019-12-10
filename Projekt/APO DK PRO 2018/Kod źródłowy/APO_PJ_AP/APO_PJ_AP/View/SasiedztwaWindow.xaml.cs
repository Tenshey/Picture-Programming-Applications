using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using APO_PJ_AP.ViewModel;

namespace APO_PJ_AP.View
{
    /// <summary>
    /// Interaction logic for SasiedztwaWindow.xaml
    /// </summary>
    public partial class SasiedztwaWindow : Window
    {
        public SasiedztwaViewModel sasiedztwaViewModel;
        public SasiedztwaWindow(Bitmap bmp, int value)
        {
            sasiedztwaViewModel = new SasiedztwaViewModel(bmp, value);
            InitializeComponent();
            DataContext = sasiedztwaViewModel;
        }

        private void DataGrid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^-0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= DataGrid_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += DataGrid_RowEditEnding;
            }
            else return;

            if ((sender as DataGrid).CurrentColumn != null)
            {
                int columnIndex = (sender as DataGrid).CurrentColumn.DisplayIndex;
                int rowIndex = (sender as DataGrid).SelectedIndex;
                DataRowView dataRow = (DataRowView)(sender as DataGrid).SelectedItem;
                int cellValue = int.Parse(dataRow.Row.ItemArray[columnIndex].ToString());

                if (sasiedztwaViewModel.MaskValues.Values[columnIndex, rowIndex] != cellValue)
                    sasiedztwaViewModel.MaskValues.Values[columnIndex, rowIndex] = cellValue;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sasiedztwaViewModel.FillWindow(sasiedztwaViewModel.WhichMethod);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sasiedztwaViewModel.getDataTable(sasiedztwaViewModel.SelectedItem);
            if (sasiedztwaViewModel.WhichMethod == 3 )
                sasiedztwaViewModel.getDataTable2(sasiedztwaViewModel.SelectedItem);
            else
                sasiedztwaViewModel.getDataTable2(null);
            if (sasiedztwaViewModel.WhichMethod == 5)
                sasiedztwaViewModel.fillMaskTemplatesMetodaUzgadnianiaWzorca();
            sasiedztwaViewModel.FillWindow(sasiedztwaViewModel.WhichMethod);
        }

    }
}
