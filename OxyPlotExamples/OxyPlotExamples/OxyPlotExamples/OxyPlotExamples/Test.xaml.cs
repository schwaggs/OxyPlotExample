using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OxyPlotExamples
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Test : ContentPage
    {
        #region Class Data
        public PlotModel model { get; set; }
        public ReadingCell.ReadingUnitType CurrentUnit { get; set; }
        public LineSeries temp1 { get; set; }
        public LineSeries temp2 { get; set; }
        public LineSeries temp3 { get; set; }

        #region Configuration Data

        OxyColor Temp_LineColor { get; set; }
        OxyColor Humid_LineColor { get; set; }
        OxyColor SP_LineColor { get; set; }

        #endregion

        public DateTimeAxis Horizontal_Axis { get; set; }
        public LinearAxis Vertical_Axis { get; set; }


        #region Full Data Sets

        public List<DataPoint> Temp_Data { get; set; }
        public List<DataPoint> Humid_Data { get; set; }
        public List<DataPoint> SP_Data { get; set; }

        #endregion

        #region Individual Views

        // The series used in each view, just with different axis

        public LineSeries Temp_View { get; set; }
        public LineSeries Humid_View { get; set; }
        public LineSeries SP_View { get; set; }

        #endregion

        #region OxyPlot Elements

        public PlotModel ChartModel
        {
            get
            {
                return new PlotModel
                {
                    Background = OxyColors.White,
                    PlotAreaBackground = OxyColors.White,
                    PlotType = PlotType.XY,
                    TitleColor = OxyColors.Black,
                    PlotMargins = new OxyThickness(50, 0, 30, 50)
                };
            }
        }

        public DateTimeAxis HorizontalAxis
        {
            get
            {
                return new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    AxisTitleDistance = 10,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                    IntervalType = DateTimeIntervalType.Hours,
                    StringFormat = "h tt"
                };
            }
        }

        public LinearAxis VerticalAxis
        {
            get
            {
                return new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    Maximum = 100,
                    AbsoluteMinimum = 0,
                    AbsoluteMaximum = 100,
                    Title = "Measurement",
                    Unit = "°F",
                    AxisTitleDistance = 10,
                    IsPanEnabled = false,
                    IsZoomEnabled = false
                };
            }
        }

        #endregion

        #endregion

        public Test()
        {
            //PlotController cont = new PlotController();
            //cont.BindTouchDown(PlotCommands.PointsOnlyTrackTouch);

            InitializeComponent();

            DataManager.Init();
            CurrentUnit = ReadingCell.ReadingUnitType.Fahrenheit;

            model = ChartModel;

            model.TouchStarted += Model_TouchStarted;

            Temp_LineColor = OxyColors.Blue;
            Humid_LineColor = OxyColors.Red;
            SP_LineColor = OxyColors.Orange;

            Init(30);

            model.Axes.Add(Horizontal_Axis);
            model.Axes.Add(Vertical_Axis);

            DataManager.Get_Current_Day(CurrentUnit);
            Get_Series();

            model.Series.Add(temp1);
            model.Series.Add(temp2);
            model.Series.Add(temp3);

            chart1.Model = model;

            PlotTouch.Set_Margins(chart1.Model.PlotMargins.Left, 15, chart1.Model.PlotMargins.Right);
        }

        #region Initialization

        public void Init(int num_days)
        {

            Horizontal_Axis = HorizontalAxis;
            Vertical_Axis = VerticalAxis;
        }

        public void Series_Init()
        {
            Temp_View = new LineSeries
            {
                Color = Temp_LineColor,
                MarkerFill = OxyColors.Transparent,
                MarkerType = MarkerType.Triangle
            };



            Humid_View = new LineSeries
            {
                Color = Humid_LineColor,
                MarkerFill = OxyColors.Transparent,
                MarkerType = MarkerType.Triangle
            };



            SP_View = new LineSeries
            {
                Color = SP_LineColor,
                MarkerFill = OxyColors.Transparent,
                MarkerType = MarkerType.Triangle
            };


        }

        #endregion

        #region Events

        private void Model_TouchStarted(object sender, OxyTouchEventArgs e)
        {
            PlotTouch.Panning = false;
            PlotTouch.Initial_Touch.X_Location = e.Position.X;
            PlotTouch.Initial_Touch.Start_Time = DateTime.UtcNow;
            int index = PlotTouch.Index;


        }

        #region Orientation Change

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (PlotTouch.Current_Orientation == PlotTouch.Orientation.None)
            {
                PlotTouch.Current_Screen_Width = width;

                if (width > height)
                {
                    PlotTouch.Current_Orientation = PlotTouch.Orientation.Horizontal;
                }

                else if (height > width)
                {
                    PlotTouch.Current_Orientation = PlotTouch.Orientation.Vertical;
                }

                chart1.Model.InvalidatePlot(false);
            }

            else if (width > height && PlotTouch.Current_Orientation != PlotTouch.Orientation.Horizontal)
            {
                PlotTouch.Current_Orientation = PlotTouch.Orientation.Horizontal;
                PlotTouch.Current_Screen_Width = width;

                chart1.Model.InvalidatePlot(false);
            }

            else if (height > width && PlotTouch.Current_Orientation != PlotTouch.Orientation.Vertical)
            {
                PlotTouch.Current_Orientation = PlotTouch.Orientation.Vertical;
                PlotTouch.Current_Screen_Width = width;

                chart1.Model.InvalidatePlot(false);
            }
        }

        #endregion


        #region Tab Touch

        private void Day_Tapped(object sender, EventArgs e)
        {
            chart1.Model.Series.Clear();
            chart1.Model.Axes.Clear();
            PlotTouch.Current_Interval = PlotTouch.IntervalType.Hour;

            chart1.Model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                AxisTitleDistance = 10,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                IntervalType = DateTimeIntervalType.Hours,
                StringFormat = "h tt"
            });

            chart1.Model.Axes.Add(VerticalAxis);

            DataManager.Get_Current_Day(CurrentUnit);
            Get_Series();

            chart1.Model.Series.Add(temp1);
            chart1.Model.Series.Add(temp2);
            chart1.Model.Series.Add(temp3);
            chart1.Model.InvalidatePlot(true);
        }

        private void Week_Tapped(object sender, EventArgs e)
        {
            chart1.Model.Series.Clear();
            chart1.Model.Axes.Clear();
            PlotTouch.Current_Interval = PlotTouch.IntervalType.Week;

            chart1.Model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                AxisTitleDistance = 10,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "M/d"
            });

            chart1.Model.Axes.Add(VerticalAxis);

            DataManager.Get_Current_Week(CurrentUnit);
            Get_Series();

            chart1.Model.Series.Add(temp1);
            chart1.Model.Series.Add(temp2);
            chart1.Model.Series.Add(temp3);
            chart1.Model.InvalidatePlot(true);
        }

        private void Month_Tapped(object sender, EventArgs e)
        {
            chart1.Model.Series.Clear();
            chart1.Model.Axes.Clear();
            PlotTouch.Current_Interval = PlotTouch.IntervalType.Month;

            chart1.Model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                AxisTitleDistance = 10,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "M/d"
            });

            chart1.Model.Axes.Add(VerticalAxis);

            DataManager.Get_Current_Month(CurrentUnit);
            Get_Series();

            chart1.Model.Series.Add(temp1);
            chart1.Model.Series.Add(temp2);
            chart1.Model.Series.Add(temp3);
            chart1.Model.InvalidatePlot(true);
        }

        private void Chart1_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    PlotTouch.Panning = true;
                    PlotTouch.Running_Touch.X_Location = e.TotalX + PlotTouch.Initial_Touch.X_Location;
                    PlotTouch.Running_Touch.X_Change = e.TotalX;

                    int index = PlotTouch.Index;

                    break;

                case GestureStatus.Completed:

                    PlotTouch.Panning = false;
                    PlotTouch.Final_Touch.Start_Time = DateTime.UtcNow;
                    PlotTouch.Final_Touch.X_Location = PlotTouch.Running_Touch.X_Location;
                    PlotTouch.Final_Touch.X_Change = PlotTouch.Running_Touch.X_Change;

                    if (PlotTouch.Is_Swipe())
                    {
                        // Swipped left <= Increase view
                        if (PlotTouch.Final_Touch.X_Change < 0)
                        {
                            // Increase the day, week, month
                            switch (PlotTouch.Current_Interval)
                            {
                                case PlotTouch.IntervalType.Hour:

                                    DataManager.Move_Day(true);

                                    chart1.Model.Title = DataManager.Get_Current_Day_DateTime().ToString("MMMM d, yyyy");

                                    chart1.Model.Series.Clear();

                                    DataManager.Get_Current_Day(CurrentUnit);
                                    Get_Series();

                                    chart1.Model.Series.Add(temp1);
                                    chart1.Model.Series.Add(temp2);
                                    chart1.Model.Series.Add(temp3);
                                    chart1.Model.InvalidatePlot(true);

                                    break;

                                case PlotTouch.IntervalType.Week:

                                    DataManager.Move_Week(true);

                                    DateTime start = DataManager.Get_Current_Week_DateTime();
                                    DateTime end = start.AddDays(7);

                                    chart1.Model.Title = start.ToString("M/d/yy") + " - " + end.ToString("M/d/yy");

                                    chart1.Model.Series.Clear();

                                    DataManager.Get_Current_Week(CurrentUnit);
                                    Get_Series();

                                    chart1.Model.Series.Add(temp1);
                                    chart1.Model.Series.Add(temp2);
                                    chart1.Model.Series.Add(temp3);
                                    chart1.Model.InvalidatePlot(true);

                                    break;

                                case PlotTouch.IntervalType.Month:

                                    DataManager.Move_Month(true);

                                    chart1.Model.Title = DataManager.Get_Current_Month_DateTime().ToString("MMMM yyyy");

                                    chart1.Model.Series.Clear();

                                    DataManager.Get_Current_Month(CurrentUnit);
                                    Get_Series();

                                    chart1.Model.Series.Add(temp1);
                                    chart1.Model.Series.Add(temp2);
                                    chart1.Model.Series.Add(temp3);
                                    chart1.Model.InvalidatePlot(true);

                                    break;
                            }
                        }

                        // Swipped right => Decrease view
                        else if (PlotTouch.Final_Touch.X_Change > 0)
                        {
                            // Decrease the day, week, month
                            switch (PlotTouch.Current_Interval)
                            {
                                case PlotTouch.IntervalType.Hour:

                                    DataManager.Move_Day(false);

                                    chart1.Model.Title = DataManager.Get_Current_Day_DateTime().ToString("MMMM d, yyyy");

                                    chart1.Model.Series.Clear();

                                    DataManager.Get_Current_Day(CurrentUnit);
                                    Get_Series();

                                    chart1.Model.Series.Add(temp1);
                                    chart1.Model.Series.Add(temp2);
                                    chart1.Model.Series.Add(temp3);
                                    chart1.Model.InvalidatePlot(true);

                                    break;

                                case PlotTouch.IntervalType.Week:

                                    DataManager.Move_Week(false);

                                    DateTime start = DataManager.Get_Current_Week_DateTime();
                                    DateTime end = start.AddDays(7);

                                    chart1.Model.Title = start.ToString("M/d/yy") + " - " + end.ToString("M/d/yy");

                                    chart1.Model.Series.Clear();

                                    DataManager.Get_Current_Week(CurrentUnit);
                                    Get_Series();

                                    chart1.Model.Series.Add(temp1);
                                    chart1.Model.Series.Add(temp2);
                                    chart1.Model.Series.Add(temp3);
                                    chart1.Model.InvalidatePlot(true);

                                    break;

                                case PlotTouch.IntervalType.Month:

                                    DataManager.Move_Month(false);
                                    
                                    chart1.Model.Title = DataManager.Get_Current_Month_DateTime().ToString("MMMM yyyy");

                                    chart1.Model.Series.Clear();

                                    DataManager.Get_Current_Month(CurrentUnit);
                                    Get_Series();

                                    chart1.Model.Series.Add(temp1);
                                    chart1.Model.Series.Add(temp2);
                                    chart1.Model.Series.Add(temp3);
                                    chart1.Model.InvalidatePlot(true);

                                    break;
                            }
                        }
                    }

                    break;
            }
        }

        private void TemperatureUnit_Farenheit_Clicked(object sender, EventArgs e)
        {
            if (chart1.Model.Axes[1].Unit == "°C")
            {
                CurrentUnit = ReadingCell.ReadingUnitType.Fahrenheit;

                switch (PlotTouch.Current_Interval)
                {
                    case PlotTouch.IntervalType.Hour:

                        DataManager.Get_Current_Day(CurrentUnit);

                        break;

                    case PlotTouch.IntervalType.Week:

                        DataManager.Get_Current_Week(CurrentUnit);

                        break;

                    case PlotTouch.IntervalType.Month:

                        DataManager.Get_Current_Month(CurrentUnit);

                        break;
                }

                chart1.Model.Axes[1].Unit = "°F";
                chart1.Model.Series.Clear();

                Get_Series();

                chart1.Model.Series.Add(temp1);
                chart1.Model.Series.Add(temp2);
                chart1.Model.Series.Add(temp3);

                chart1.Model.InvalidatePlot(true);
            }
        }

        private void Get_Series()
        {
            temp1 = new LineSeries();
            temp2 = new LineSeries();
            temp3 = new LineSeries();

            foreach (DataPoint dp in DataManager.Temperature_Readings)
            {
                temp1.Points.Add(dp);
            }

            foreach (DataPoint dp in DataManager.Humidity_Readings)
            {
                temp2.Points.Add(dp);
            }

            foreach (DataPoint dp in DataManager.SetPoint_Readings)
            {
                temp3.Points.Add(dp);
            }
        }

        private void TemperatureUnit_Celcius_Clicked(object sender, EventArgs e)
        {
            if (chart1.Model.Axes[1].Unit == "°F")
            {
                CurrentUnit = ReadingCell.ReadingUnitType.Celcius;

                chart1.Model.Axes[1].Unit = "°C";
                chart1.Model.Series.Clear();

                switch (PlotTouch.Current_Interval)
                {
                    case PlotTouch.IntervalType.Hour:

                        DataManager.Get_Current_Day(CurrentUnit);

                        break;

                    case PlotTouch.IntervalType.Week:

                        DataManager.Get_Current_Week(CurrentUnit);

                        break;

                    case PlotTouch.IntervalType.Month:

                        DataManager.Get_Current_Month(CurrentUnit);

                        break;
                }

                Get_Series();

                chart1.Model.Series.Add(temp1);
                chart1.Model.Series.Add(temp2);
                chart1.Model.Series.Add(temp3);

                chart1.Model.InvalidatePlot(true);
            }
        }

        private double Farenheit2Celcius(double Degrees)
        {
            return (Degrees - 32) / 1.8;
        }

        private double Celcius2Farenheit(double Degrees)
        {
            return (Degrees * 1.8) + 32;
        }
        #endregion

        private void Data_30_Clicked(object sender, EventArgs e)
        {

        }

        private void Data_60_Clicked(object sender, EventArgs e)
        {

        }

        private void Data_90_Clicked(object sender, EventArgs e)
        {

        }


        #endregion

        private void DataViewer_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DataViewer());
        }
    }
}