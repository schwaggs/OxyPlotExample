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
using Xamarin.Forms.Xaml;

/*
 *  Scaling issues when zooming in where line graph completely disappears.
 */

namespace OxyPlotExamples
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoreCharts : ContentPage
	{
        public PlotModel model { get; set; }

		public MoreCharts ()
		{
			InitializeComponent ();

            model = Get_Model();
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(new DateTime(20, 1, 1)),
                Maximum = DateTimeAxis.ToDouble(new DateTime(2007, 1, 1)),
                Title = "DateTimeAxis"
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left
            });
            model.Series.Add(Get_Series(10));

            chart1.BindingContext = this;
        }

        private LineSeries Get_Series(int num_days)
        {
            LineSeries temp = new LineSeries();

            DateTime start = DateTime.Today.AddDays(num_days * -1);
            Random r = new Random();

            while(start != DateTime.Today)
            {
                for(int i = 0; i < 23; i ++)
                {
                    temp.Points.Add(new DataPoint
                    (
                        DateTimeAxis.ToDouble(start.AddHours(i)),
                        r.Next(0, 100)
                    ));
                }

                start = start.AddDays(1);
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
                Title = "DateTime  Axis",
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