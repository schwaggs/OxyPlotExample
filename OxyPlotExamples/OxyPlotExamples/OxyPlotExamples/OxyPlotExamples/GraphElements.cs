using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OxyPlotExamples
{
    public class GraphElements
    {
        private DateTime StartDate;
        private DateTime EndDate;

        private int SetPoint = 72;

        public double AxesMinValue
        {
            get
            {
                return DateTimeAxis.ToDouble(StartDate);
            }
        }

        public double AxesMaxValue
        {
            get
            {
                return DateTimeAxis.ToDouble(EndDate);
            }
        }

        public DateTimeAxis HorizontalAxes
        {
            get
            {
                return new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    Minimum = AxesMinValue,
                    Maximum = AxesMaxValue,
                    StringFormat = OutputFormatType2String(OutputFormatType.ShortMonthDaySlash),
                    MajorStep = double.NaN,
                    MajorTickSize = 7
                };
            }
        }

        public LinearAxis VerticalAxes
        {
            get
            {
                return new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = SetPoint - 4,
                    Maximum = SetPoint + 4,
                    MajorStep = double.NaN,
                    MajorTickSize = 0.5
                };
            }
        }

        public string Tracker
        {
            get
            {
                return "";
            }
        }

        public PlotModel Model
        {
            get
            {
                PlotModel temp = new PlotModel();
                temp.PlotType = PlotType.XY;
                temp.Series.Add(GeneratePoints(72));
                temp.Axes.Add(HorizontalAxes);
                temp.Axes.Add(VerticalAxes);

                return temp;
            }
        }

        public PlotView View
        {
            get
            {
                return new PlotView
                {
                    Model = Model,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
            }
        }

        public string Superscript(string Base, string Upper)
        {
            return string.Format("{0}^{1}", Base, Upper);
        }

        public string Subscript(string Base, string Lower)
        {
            return string.Format("{0}_{1}", Base, Lower);
        }

        public string Degrees(string val, string unit)
        {
            return string.Format("{0}° {1}", val, unit);
        }

        public GraphElements(int num_days_from_today)
        {
            StartDate = DateTime.Today.AddDays(num_days_from_today * -1);
            EndDate = DateTime.Now;
        }

        public GraphElements(DateTime start, DateTime end)
        {
            if(start != null)
            {
                StartDate = start;
            }

            else
            {
                StartDate = DateTime.Now;
            }

            if(end != null)
            {
                EndDate = end;
            }

            else
            {
                EndDate = DateTime.Now;
            }
        }

        public enum OutputFormatType
        {
            ShortYear,
            LongYear,
            ShortMonth,
            MidMonth,
            LongMonth,
            ShortWeek,
            LongWeek,
            Day,
            ShortHour,
            LongHour,
            Minute,
            Second,
            YearMonthDayDash,
            MonthDayYearSlash,
            ShortMonthDaySlash
        }

        public string OutputFormatType2String(OutputFormatType output)
        {
            switch(output)
            {
                case OutputFormatType.Second:
                    return "ss";

                case OutputFormatType.Minute:
                    return "mm";

                case OutputFormatType.ShortHour:
                    return "hh";

                case OutputFormatType.LongHour:
                    return "HH";

                case OutputFormatType.Day:
                    return "dd";

                case OutputFormatType.ShortWeek:
                    return "w";

                case OutputFormatType.LongWeek:
                    return "ww";

                case OutputFormatType.ShortMonth:
                    return "MM";

                case OutputFormatType.MidMonth:
                    return "MMM";

                case OutputFormatType.LongMonth:
                    return "MMMM";

                case OutputFormatType.ShortYear:
                    return "yy";

                case OutputFormatType.LongYear:
                    return "yyyy";

                case OutputFormatType.MonthDayYearSlash:
                    return "MM/dd/yyyy";

                case OutputFormatType.YearMonthDayDash:
                    return "yyyy-MM-dd";

                case OutputFormatType.ShortMonthDaySlash:
                    return "M/d";

                default:
                    return string.Empty;
            }
        }

        public LineSeries GeneratePoints(int SetPoint)
        {
            LineSeries result = new LineSeries();
            DateTime start = StartDate;

            Random r1 = new Random();

            while(start != EndDate)
            {
                for(int i = 0; i < 24; i ++)
                {
                    result.Points.Add(new DataPoint
                    (
                        DateTimeAxis.ToDouble(start.AddDays(i)),
                        SetPoint + r1.Next(-2, 2)
                    ));
                }

                start = start.AddDays(1);
            }

            return result;
        }
    }
}
