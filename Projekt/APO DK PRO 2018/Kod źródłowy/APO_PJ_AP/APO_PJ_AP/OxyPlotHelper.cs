using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace APO_PJ_AP
{
    public static class OxyPlotHelper
    {
        public static void Plot3SeriaKolumnowy(string xTitle, string yTitle, OxyColor color1, OxyColor color2,
    OxyColor color3, int[,] histogramValues, string label1, string label2, string label3, PlotModel histogramModel,
    bool chkR, bool chkG, bool chkB)
        {
            histogramModel.Series.Clear();
            histogramModel.Axes.Clear();
            var l = histogramValues.Length / 3;
            var categoryAxisRGB = new CategoryAxis
            {
                GapWidth = 0,
                IsAxisVisible = false,
                IsZoomEnabled = false,
                MinorStep = 1
            };
            var s1 = new ColumnSeries
            {
                Title = label1,
                StrokeColor = color1,
                FillColor = color1,
                StrokeThickness = 1
            };

            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, 0];
                categoryAxisRGB.Labels.Add(i.ToString());
                s1.Items.Add(new ColumnItem { Value = y_val, Color = color1 });
            }

            var s2 = new ColumnSeries { Title = label2, StrokeColor = color2, FillColor = color2, StrokeThickness = 1 };
            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, 1];
                s2.Items.Add(new ColumnItem { Value = y_val, Color = color2 });
            }

            var s3 = new ColumnSeries { Title = label3, StrokeColor = color3, FillColor = color3, StrokeThickness = 1 };
            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, 2];
                s3.Items.Add(new ColumnItem { Value = y_val, Color = color3 });
            }

            var linearAxisRGB1 = new LinearAxis
            {
                Maximum = l,
                Minimum = 0,
                IsZoomEnabled = false,
                Position = AxisPosition.Bottom,
                Title = xTitle
            };

            var linearAxisRGB2 = new LinearAxis
            {
                AbsoluteMinimum = 0,
                MinimumPadding = 0,
                IsZoomEnabled = false,
                Title = yTitle
            };

            histogramModel.Axes.Add(categoryAxisRGB);
            histogramModel.Axes.Add(linearAxisRGB1);
            histogramModel.Axes.Add(linearAxisRGB2);
            if (chkR)
                histogramModel.Series.Add(s1);
            if (chkG)
                histogramModel.Series.Add(s2);
            if (chkB)
                histogramModel.Series.Add(s3);
            histogramModel.InvalidatePlot(true);
        }

        public static void Plot1SeriaColumn(string xTitle, string yTitle, OxyColor color, int[,] pointList,
            string label, PlotModel plotModel, int kolor)
        {
            plotModel.Series.Clear();
            plotModel.Axes.Clear();
            var l = pointList.Length / 3;
            var categoryAxis = new CategoryAxis
            {
                GapWidth = 0,
                IsAxisVisible = false,
                IsZoomEnabled = false,
                MinorStep = 1
            };
            var series = new ColumnSeries { Title = label, StrokeColor = color, FillColor = color, StrokeThickness = 1 };
            for (int i = 0; i < l; i++)
            {
                int y_val = pointList[i, kolor];
                categoryAxis.Labels.Add(i.ToString());
                series.Items.Add(new ColumnItem { Value = y_val, Color = color });
            }

            var linearAxis = new LinearAxis
            {
                Maximum = l,
                Minimum = 0,
                IsZoomEnabled = false,
                Position = AxisPosition.Bottom,
                Title = xTitle
            };

            var linearAxis2 = new LinearAxis
            {
                AbsoluteMinimum = 0,
                MinimumPadding = 0,
                IsZoomEnabled = false,
                Title = yTitle
            };


            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(linearAxis);
            plotModel.Axes.Add(linearAxis2);
            plotModel.Series.Add(series);
            plotModel.InvalidatePlot(true);
        }
        public static void Plot1Seria(string xTitle, string yTitle, OxyColor color1, PlotModel plotModel,
            int kolor, int[,] histogramValues)
        {
            plotModel.Series.Clear();
            plotModel.Axes.Clear();

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = xTitle });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = yTitle });

            var R_Series = new LineSeries { StrokeThickness = 2, MarkerSize = 2, Color = color1 };
            var l = histogramValues.Length / 3;
            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, kolor];
                R_Series.Points.Add(new DataPoint(i, y_val));
            }

            plotModel.Series.Add(R_Series);
            plotModel.InvalidatePlot(true);
        }

        public static void Plot3Serie(string xTitle, string yTitle, OxyColor color1, OxyColor color2, OxyColor color3, string label1, string label2, string label3, PlotModel plotModel, int[,] histogramValues)
        {
            plotModel.Series.Clear();
            plotModel.Axes.Clear();
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = xTitle });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = yTitle });
            var l = histogramValues.Length / 3;
            var plotR_Series = new LineSeries { Title = label1, StrokeThickness = 2, MarkerSize = 2, Color = color1 };
            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, 0];
                plotR_Series.Points.Add(new DataPoint(i, y_val));
            }

            plotModel.Series.Add(plotR_Series);

            var plotG_Series = new LineSeries { Title = label2, StrokeThickness = 2, MarkerSize = 2, Color = color2 };
            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, 1];
                plotG_Series.Points.Add(new DataPoint(i, y_val));
            }

            plotModel.Series.Add(plotG_Series);
            var plotB_Series = new LineSeries { Title = label3, StrokeThickness = 2, MarkerSize = 2, Color = color3 };
            for (int i = 0; i < l; i++)
            {
                int y_val = histogramValues[i, 2];
                plotB_Series.Points.Add(new DataPoint(i, y_val));
            }

            plotModel.Series.Add(plotB_Series);
            plotModel.InvalidatePlot(true);
        }

    }
}
