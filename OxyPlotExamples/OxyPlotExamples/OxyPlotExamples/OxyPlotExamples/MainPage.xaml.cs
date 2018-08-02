using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OxyPlotExamples
{
    public partial class MainPage : ContentPage
    {
        private Grid ChartHolder { get; set; }
        private PlotModel Plot { get; set; }
        private PlotView PlotV { get; set; }
        private LineSeries PlotSeries { get; set; }
        private Button other { get; set; }

        public MainPage()
        {
            other = new Button
            {
                Text = "XAML"
            };

            other.Clicked += Other_Clicked;

            ChartHolder = Get_Grid(2, 1);
            Plot = Get_Model();
            PlotV = Get_View();
            PlotSeries = Get_Series();
            Plot.Series.Add(PlotSeries);
            PlotV.Model = Plot;

            ChartHolder.Children.Add(PlotV, 0, 0);
            ChartHolder.Children.Add(other, 0, 1);

            Content = ChartHolder;
        }

        private void Other_Clicked(object sender, EventArgs e)
        {
            MoreCharts temp = new MoreCharts();
            Navigation.PushModalAsync(temp);
        }

        private LineSeries Get_Series()
        {
            LineSeries temp = new LineSeries();
            temp.Points.Add(new DataPoint(0, 0));
            temp.Points.Add(new DataPoint(1, 1));
            temp.Points.Add(new DataPoint(2, 2));
            temp.Points.Add(new DataPoint(3, 3));
            temp.Points.Add(new DataPoint(4, 4));
            temp.Points.Add(new DataPoint(5, 5));

            return temp;
        }

        private Grid Get_Grid(int num_rows, int num_cols)
        {
            Grid temp = new Grid();

            for(int i = 0; i < num_rows; i ++)
            {
                temp.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < num_cols; i++)
            {
                temp.ColumnDefinitions.Add(new ColumnDefinition());
            }

            return temp;
        }

        private PlotModel Get_Model()
        {
            PlotModel temp = new PlotModel
            {
                Background = OxyColors.White,
                PlotAreaBackground = OxyColors.White,
                PlotType = PlotType.Cartesian,
                Title = "Model",
                TitleColor = OxyColors.Black
            };

            return temp;
        }

        private PlotView Get_View()
        {
            PlotView temp = new PlotView
            {
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            return temp;
        }
    }
}