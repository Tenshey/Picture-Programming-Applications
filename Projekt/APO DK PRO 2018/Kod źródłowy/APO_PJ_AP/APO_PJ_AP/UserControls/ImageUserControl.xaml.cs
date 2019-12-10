using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using APO_PJ_AP.View;
using APO_PJ_AP.ViewModel;
using Color = System.Drawing.Color;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;
using UserControl = System.Windows.Controls.UserControl;

namespace APO_PJ_AP.UserControls
{
    /// <summary>
    /// Interaction logic for ImageUserControl.xaml
    /// </summary>
    public partial class ImageUserControl : UserControl
    {
        public static readonly DependencyProperty SourceBitmapProperty = DependencyProperty.Register(nameof(SourceBitmap), typeof(Bitmap), typeof(ImageUserControl), new FrameworkPropertyMetadata(null, ValueChanged));
        public static readonly DependencyProperty PointListProperty = DependencyProperty.Register(nameof(PointList), typeof(int[,]), typeof(ImageUserControl));
        private Point StartPoint;
        private Point EndPoint;
        private bool MouseIsDown;
        DataGridView mtbDate;

        public ImageUserControl()
        {
            InitializeComponent();
            Image.MouseMove += Image_MouseMove;
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
                ImageTabControl.Visibility = Visibility.Visible;
            }
            else
            {
                ImageTabControl.Visibility = Visibility.Hidden;
            }
        }

        public Bitmap SourceBitmap
        {

            get => (Bitmap)GetValue(SourceBitmapProperty);
            set => SetValue(SourceBitmapProperty, value);
        }

        public bool IsLineEnabled;

        public int[,] PointList
        {
            get => (int[,])GetValue(PointListProperty);
            set => SetValue(PointListProperty, value);
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ImageUserControl)d;
            if (control?.SourceBitmap != null)
            {
                control.Image.Source = WPFUtilities.SetImgSourceMemoryStream(control.SourceBitmap);
                control.ImageTabControl.Visibility = Visibility.Visible;

                control.IsLineEnabled = true;
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                IsLineEnabled = true;
            if (e.LeftButton == MouseButtonState.Pressed)
                IsLineEnabled = false;

            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                //sprawdzamy czy przycisk nie był wciśniety wcześniej
                if (!MouseIsDown)
                {
                    //jeśli nie to ustawiamy punkt początkowy i zaznaczamy, że jest wciśniety
                    StartPoint = e.GetPosition(Image);
                    MouseIsDown = true;
                }
                EndPoint = e.GetPosition(Image);
                Point tmpStartPoint = StartPoint;
                Point tmpEndPoint = EndPoint;

                //sprawdzamy czy obrazek zajmuje całą powierzchnię w zakładce w której się znajduje
                if (Math.Abs(Image.ActualHeight - gridImageContainer.ActualHeight) > 0.2f)
                {
                    double tmp = (gridImageContainer.ActualHeight - Image.ActualHeight) / 2; // dzielimy na 2, ponieważ różnica to puste miejsce z lewej i z prawej strony
                    tmpStartPoint.Y = tmpStartPoint.Y + tmp;
                    tmpEndPoint.Y = tmpEndPoint.Y + tmp;
                }
                if (Math.Abs(Image.ActualWidth - gridImageContainer.ActualWidth) > 0.2f)
                {
                    double tmp = (gridImageContainer.ActualWidth - Image.ActualWidth) / 2;
                    tmpStartPoint.X = tmpStartPoint.X + tmp;
                    tmpEndPoint.X = tmpEndPoint.X + tmp;
                }
                //
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    LineGeometry lineGeometry = new LineGeometry(tmpStartPoint, tmpEndPoint);
                    Path.Data = lineGeometry;
                }
                else if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Rect newRect = new Rect(tmpStartPoint, tmpEndPoint);
                    RectangleGeometry rectangleGeometry = new RectangleGeometry(newRect);
                    Path.Data = rectangleGeometry;
                }
            }
            else
            {
                //do tej części wchodzimy jeśli przycisk nie jest wciśniety, sprawdzamy czy był wciśnięty i jęsli był to zaznaczamy, że już nie jest
                if (MouseIsDown)
                {
                    MouseIsDown = false;
                    int x, y, lenght, width;
                    double value1, value2;

                    //Wyświetlany obrazek jest skalowany (tylko te większe obrazki) i trzeba uwzględnić to przy tworzeniu nowego obrazka.
                    value1 = Image.ActualHeight / SourceBitmap.Height;
                    value2 = Image.ActualWidth / SourceBitmap.Width;
                    StartPoint.X /= value1;
                    EndPoint.X /= value1;
                    StartPoint.Y /= value2;
                    EndPoint.Y /= value2;
                    //

                    //w obszarze, który został zaznaczony szukamy lewego górnego rogu i od tego punktu wyznaczamy wartości
                    if (!IsLineEnabled)
                    {
                        if (StartPoint.X.Equals(EndPoint.X))
                        {
                            EndPoint.X++;
                        }
                        if (StartPoint.Y.Equals(EndPoint.Y))
                        {
                            EndPoint.Y++;
                        }
                    }

                    if (StartPoint.X < EndPoint.X)
                    {
                        x = (int)StartPoint.X;
                        lenght = (int)(EndPoint.X - StartPoint.X);
                    }
                    else
                    {
                        x = (int)EndPoint.X;
                        lenght = (int)(StartPoint.X - EndPoint.X);
                    }

                    if (StartPoint.Y < EndPoint.Y)
                    {
                        y = (int)StartPoint.Y;
                        width = (int)(EndPoint.Y - StartPoint.Y);
                    }
                    else
                    {
                        y = (int)EndPoint.Y;
                        width = (int)(StartPoint.Y - EndPoint.Y);
                    }


                    if (IsLineEnabled)
                    {
                        getPointFromLine((int)StartPoint.X, (int)StartPoint.Y, (int)EndPoint.X, (int)EndPoint.Y);
                        IsLineEnabled = false;
                    }
                    else
                    {
                        CutBitmap(new Rectangle(x, y, lenght, width));
                        Path.Data = null;
                    }
                }
            }
        }

        private void RadioButtonRGBHSV_Checked(object sender, RoutedEventArgs e)
        {
            TabelaChange();
        }

        private void TabelaChange()
        {
            using (new WaitCursor())
            {
                if (SourceBitmap != null)
                {

                    bool rbR = RadioButtonR.IsChecked != null && RadioButtonR.IsChecked.Value;
                    bool rbG = RadioButtonG.IsChecked != null && RadioButtonG.IsChecked.Value;
                    bool rbB = RadioButtonB.IsChecked != null && RadioButtonB.IsChecked.Value;
                    bool rbH = RadioButtonH.IsChecked != null && RadioButtonH.IsChecked.Value;
                    bool rbS = RadioButtonS.IsChecked != null && RadioButtonS.IsChecked.Value;
                    bool rbV = RadioButtonV.IsChecked != null && RadioButtonV.IsChecked.Value;
                    if (mtbDate != null)
                    {
                        if (rbR)
                            GenerujTablice(0, SourceBitmap, mtbDate);
                        if (rbG)
                            GenerujTablice(1, SourceBitmap, mtbDate);
                        if (rbB)
                            GenerujTablice(2, SourceBitmap, mtbDate);
                        if (rbH)
                            GenerujTablice(3, SourceBitmap, mtbDate);
                        if (rbS)
                            GenerujTablice(4, SourceBitmap, mtbDate);
                        if (rbV)
                            GenerujTablice(5, SourceBitmap, mtbDate);
                    }
                }
            }
        }

        private void GenerujTablice(int k, Bitmap bmp, DataGridView dataGridTablica)
        {
            #region Generowanie Tablicy obrazu
            dataGridTablica.Visible = false;
            double h, s, v;
            FastBitmap fobrazek_bitmapa = new FastBitmap(bmp);
            EnableDoubleBuffered(dataGridTablica, true);
            fobrazek_bitmapa.LockImage();
            int cols_width = 80;
            if (dataGridTablica.Columns.Count != bmp.Width || dataGridTablica.Rows.Count != bmp.Height)
            {
                dataGridTablica.Rows.Clear();
                dataGridTablica.Columns.Clear();
                for (int x = 0; x < bmp.Width; x++)
                {
                    dataGridTablica.Columns.Add(null, x.ToString());
                    dataGridTablica.Columns[x].Width = cols_width;
                    dataGridTablica.Columns[x].FillWeight = 0.0000001f;
                }
                dataGridTablica.Rows.Add(bmp.Height);
                for (int y = 0; y < bmp.Height; y++)
                {
                    dataGridTablica.Rows[y].HeaderCell.Value = y.ToString();
                }
            }
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (k == 0)
                        dataGridTablica[x, y].Value = fobrazek_bitmapa.GetPixel(x, y).R;
                    if (k == 1)
                        dataGridTablica[x, y].Value = fobrazek_bitmapa.GetPixel(x, y).G;
                    if (k == 2)
                        dataGridTablica[x, y].Value = fobrazek_bitmapa.GetPixel(x, y).B;
                    if (k > 2)
                    {
                        hsv.ColorToHSV(fobrazek_bitmapa.GetPixel(x, y), out h, out s, out v);
                        if (k == 3)
                            dataGridTablica[x, y].Value = h.ToString(CultureInfo.InvariantCulture);
                        if (k == 4)
                            dataGridTablica[x, y].Value = s.ToString(CultureInfo.InvariantCulture);
                        if (k == 5)
                            dataGridTablica[x, y].Value = v.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
            fobrazek_bitmapa.UnlockImage();
            dataGridTablica.Visible = true;
            #endregion

        }

        private void EnableDoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public void CutBitmap(Rectangle rect)
        {
            if (rect.Height > 1 && rect.Width > 1)
            {
                Bitmap CroppedBitmap = SourceBitmap.Clone(rect, PixelFormat.DontCare);
                var newWindow = new PreviewWindow(CroppedBitmap, 0);
                var a = (PreviewViewModel)newWindow.DataContext;
                newWindow.ShowDialog();
                if (newWindow.previewViewModel.IsSaved)
                {
                    SourceBitmap = newWindow.previewViewModel.SourceBitmap;

                }

            }

        }

        //http://tech-algorithm.com/articles/drawing-line-using-bresenham-algorithm/
        public void getPointFromLine(int x, int y, int x2, int y2)
        {
            if (x.Equals(x2) && y.Equals(y2)) return;
            List<Color> pointList = new List<Color>();
            FastBitmap lockBitmap = new FastBitmap(SourceBitmap);
            lockBitmap.LockImage();
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                pointList.Add(lockBitmap.GetPixel(x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
            var pointListArray = new int[pointList.Count, 3];
            for (int i = 0; i < pointList.Count; i++)
            {
                pointListArray[i, 0] = pointList[i].R;
                pointListArray[i, 1] = pointList[i].G;
                pointListArray[i, 2] = pointList[i].B;
            }
            lockBitmap.UnlockImage();
            PointList = pointListArray;
        }

        private void ImageUc_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the interop host control.
            WindowsFormsHost host = new WindowsFormsHost();

            mtbDate = new DataGridView();
            mtbDate.ColumnHeadersHeight = 40;
            mtbDate.RowHeadersWidth = 90;
            // Assign the MaskedTextBox control as the host control's child.
            host.Child = mtbDate;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            TabelaGrid.Children.Add(host);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ImageTabControl.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    TabelaChange();
                    break;
            }
        }

        private void buttonPreview_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new PreviewWindow((Bitmap)SourceBitmap.Clone(), 1);
            var a = (PreviewViewModel)newWindow.DataContext;
            newWindow.Show();
        }
    }
}
