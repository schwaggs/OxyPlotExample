using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OxyPlotExamples
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DataViewer : ContentPage
	{
        public List<DataViewCell> CurrentList { get; set; }

		public DataViewer ()
		{
			InitializeComponent ();

            DataManager.Initialize_Bounds();

            Current_Label.Text = DataManager.Current_Day.ToString("M/d h tt");
            Get_Source(DataManager.All_Readings());
            Items_ListView.BindingContext = this;
        }

        public void Get_Source(List<DataPoint> list)
        {
            CurrentList = new List<DataViewCell>();

            foreach(DataPoint dp in list)
            {
                CurrentList.Add(new DataViewCell
                {
                    Date = DateTimeAxis.ToDateTime(dp.X).ToString("M/d h tt"),
                    Value = dp.Y.ToString()
                });
            }
        }

        private void NextDay_Clicked(object sender, EventArgs e)
        {
            //DataManager.Get_Next_Day();
            Items_ListView.ItemsSource = null;
            Get_Source(DataManager.Combine());
            Items_ListView.ItemsSource = CurrentList;
            Current_Label.Text = DataManager.Current_Day.ToString("M/d");
        }

        private void PrevDay_Clicked(object sender, EventArgs e)
        {
            //DataManager.Get_Previous_Day();
            Items_ListView.ItemsSource = null;
            Get_Source(DataManager.Combine());
            Items_ListView.ItemsSource = CurrentList;
            Current_Label.Text = DataManager.Current_Day.ToString("M/d");
        }

        private void All_Clicked(object sender, EventArgs e)
        {
            Items_ListView.ItemsSource = null;
            Get_Source(DataManager.All_Readings());
            Items_ListView.ItemsSource = CurrentList;
            //DataManager.Current_Day = DataManager.Reading_Lower_Bound;
            Current_Label.Text = DataManager.Current_Day.ToString("M/d");
        }

        private void DayButton_Clicked(object sender, EventArgs e)
        {
            Items_ListView.ItemsSource = null;

        }

        private void Weekbutton_Clicked(object sender, EventArgs e)
        {
            Items_ListView.ItemsSource = null;
        }

        private void MonthButton_Clicked(object sender, EventArgs e)
        {
            Items_ListView.ItemsSource = null;
        }
    }

    public class DataViewCell
    {
        public string Date { get; set; }
        public string Value { get; set; }
    }
}