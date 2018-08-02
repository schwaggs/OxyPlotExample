using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OxyPlotExamples
{
    public static class DataManager
    {
        public enum DataType
        {
            Day,
            Week,
            Month
        }

        public static int setpoint = 50;
        public static int temp = 72;
        public static int offset = 3;

        public static DateTime Reading_Lower_Bound { get; set; }
        public static DateTime Reading_Upper_Bound { get; set; }

        public static List<DateTime> Start_Days { get; set; }
        public static List<DateTime> Week_Start_Days { get; set; }
        public static List<DateTime> Month_Start_Days { get; set; }

        public static int Current_Day { get; set; }
        public static int Current_Week { get; set; }
        public static int Current_Month { get; set; }
        private static int Total_Days { get; set; }
        private static int Total_Weeks { get; set; }
        private static int Total_Months { get; set; }
        private static int Leftover_Week_Days { get; set; }
        private static int Leftover_Month_Days { get; set; }

        private static List<RawReading> Raw_Readings { get; set; }
        private static List<ReadingCell> Readings { get; set; }
        public static List<DataPoint> Temperature_Readings { get; set; }
        public static List<DataPoint> Humidity_Readings { get; set; }
        public static List<DataPoint> SetPoint_Readings { get; set; }
        public static DataType CurrentDataType { get; set; }

        public static List<DataPoint> Combine()
        {
            List<DataPoint> temp = new List<DataPoint>();
            for(int i = 0; i < Temperature_Readings.Count; i ++)
            {
                temp.Add(Temperature_Readings[i]);
                temp.Add(Humidity_Readings[i]);
                temp.Add(SetPoint_Readings[i]);
            }

            return temp;
        }

        #region Initialization
        
        public static void Init()
        {
            Initialize_Raw_Readings();
            Initialize_Readings();
            Initialize_Bounds();
            Initialize_Dates();

            Current_Day = 0;
            Current_Week = 0;
            Current_Month = 0;
        }

        public static void Initialize_Readings()
        {
            Readings = new List<ReadingCell>();

            foreach (RawReading reading in Raw_Readings)
            {
                Readings.Add(new ReadingCell(reading));
            }

            Reading_Upper_Bound = Readings[Readings.Count - 1].Occurence;
            Reading_Lower_Bound = Readings[0].Occurence;
        }

        private static void Initialize_Raw_Readings()
        {
            Raw_Readings = new List<RawReading>();

            // Grab readings from azure, for now use static values and at maximum points = 90 days
            // 90 days = 2160 data points

            DateTime start = DateTime.Today.AddDays(-90);
            Random r = new Random();
            
            for (int i = 0; i < 2160; i++)
            {
                Raw_Readings.Add(new RawReading
                {
                    Set_Point = setpoint.ToString(),
                    Humidity = (setpoint + r.Next(-1*offset, offset)).ToString(),
                    Temperature = (temp + r.Next(-1*offset, offset)).ToString(),
                    Occurence = start.AddHours(i).ToString()
                });
            }
            
        }

        public static void Initialize_Bounds()
        {
            int Total_Points = Raw_Readings.Count;

            Total_Days = Total_Points / 24; // 24 hours in a day
            Total_Weeks = Total_Days / 7;   // 7 days in a week
            Total_Months = Total_Days / 30; // 30 days in a month

            Leftover_Week_Days = Total_Days - (Total_Weeks * 7);
            Leftover_Month_Days = Total_Days - (Total_Months * 30);

            if(Leftover_Week_Days > 0)
            {
                Total_Weeks++;
            }

            if(Leftover_Month_Days > 0)
            {
                Total_Months++;
            }
        }

        public static void Initialize_Dates()
        {
            Start_Days = new List<DateTime>();
            Week_Start_Days = new List<DateTime>();
            Month_Start_Days = new List<DateTime>();

            DateTime start = Reading_Lower_Bound;

            for(int i = 0; i < Total_Days; i ++)
            {
                Start_Days.Add(start);
                start = start.AddDays(1);
            }

            start = Reading_Lower_Bound;

            for(int i = 0; i < Total_Weeks; i++)
            {
                Week_Start_Days.Add(start);
                start = start.AddDays(7);
            }

            start = Reading_Lower_Bound;

            for(int i = 0; i < Total_Months; i ++)
            {
                Month_Start_Days.Add(start);
                start = start.AddMonths(1);
            }

        }

        #endregion

        #region Lists

        public static void Get_Current_Day(ReadingCell.ReadingUnitType unit)
        {
            Temperature_Readings = new List<DataPoint>();
            Humidity_Readings = new List<DataPoint>();
            SetPoint_Readings = new List<DataPoint>();

            int index = Current_Day;

            if (Current_Day == Total_Days)
            {
                index--;
            }

            DateTime start = Start_Days[index];
            DateTime end = start.AddDays(1);

            foreach (ReadingCell cell in Readings.Where(x => x.Occurence >= start && x.Occurence <= end))
            {
                Temperature_Readings.Add(new DataPoint
                (

                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Get_Temperature(unit)

                ));

                Humidity_Readings.Add(new DataPoint
                (

                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Humidity

                ));

                SetPoint_Readings.Add(new DataPoint
                (

                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Set_Point

                ));
            }
        }

        public static DateTime Get_Current_Day_DateTime()
        {
            int index = Current_Day;

            if (Current_Day == Total_Days)
            {
                index--;
            }

            return Start_Days[index];
        }

        public static void Get_Current_Week(ReadingCell.ReadingUnitType unit)
        {
            Temperature_Readings = new List<DataPoint>();
            Humidity_Readings = new List<DataPoint>();
            SetPoint_Readings = new List<DataPoint>();

            int index = Current_Week;

            if(Current_Week == Total_Weeks)
            {
                index--;
            }

            DateTime start = Week_Start_Days[index];
            DateTime end = start.AddDays(7);

            foreach (ReadingCell cell in Readings.Where(x => x.Occurence >= start && x.Occurence <= end))
            {
                Temperature_Readings.Add(new DataPoint
                (
                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Get_Temperature(unit)

                ));

                Humidity_Readings.Add(new DataPoint
                (
                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Humidity

                ));

                SetPoint_Readings.Add(new DataPoint
                (
                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Set_Point

                ));
            }
        }

        public static DateTime Get_Current_Week_DateTime()
        {
            int index = Current_Week;

            if (Current_Week == Total_Weeks)
            {
                index--;
            }

            return Week_Start_Days[index];
        }

        public static void Get_Current_Month(ReadingCell.ReadingUnitType unit)
        {
            Temperature_Readings = new List<DataPoint>();
            Humidity_Readings = new List<DataPoint>();
            SetPoint_Readings = new List<DataPoint>();

            int index = Current_Month;

            if (Current_Month == Total_Months)
            {
                index--;
            }

            DateTime start = Month_Start_Days[index];
            DateTime end = start.AddMonths(1);

            foreach (ReadingCell cell in Readings.Where(x => x.Occurence >= start && x.Occurence <= end))
            {
                Temperature_Readings.Add(new DataPoint
                (

                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Get_Temperature(unit)

                ));

                Humidity_Readings.Add(new DataPoint
                (

                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Humidity

                ));

                SetPoint_Readings.Add(new DataPoint
                (

                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Set_Point

                ));
            }
        }

        public static DateTime Get_Current_Month_DateTime()
        {
            int index = Current_Month;

            if (Current_Day == Total_Months)
            {
                index--;
            }

            return Month_Start_Days[index];
        }

        public static void Move_Day(bool increment)
        {
            if (increment)
            {
                if (Current_Day < Total_Days)
                {
                    Current_Day++;
                }
            }

            else
            {
                if(Current_Day > 0)
                {
                    Current_Day--;
                }
            }
        }

        public static void Move_Week(bool increment)
        {
            if (increment)
            {
                if (Current_Week < Total_Weeks)
                {
                    Current_Week++;
                }
            }

            else
            {
                if (Current_Week > 0)
                {
                    Current_Week--;
                }
            }
        }

        public static void Move_Month(bool increment)
        {
            if (increment)
            {
                if (Current_Month < Total_Months)
                {
                    Current_Month++;
                }
            }

            else
            {
                if (Current_Month > 0)
                {
                    Current_Month--;
                }
            }
        }
        
        public static List<DataPoint> All_Readings()
        {
            List<DataPoint> temp = new List<DataPoint>();

            foreach(ReadingCell cell in Readings)
            {
                temp.Add(new DataPoint
                (
                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Get_Temperature(ReadingCell.ReadingUnitType.Fahrenheit)
                ));

                temp.Add(new DataPoint
                (
                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Humidity
                ));

                temp.Add(new DataPoint
                (
                    DateTimeAxis.ToDouble(cell.Occurence),
                    cell.Set_Point
                ));
            }

            return temp;
        }

        // Used with all settings => 1 day = 24 hours
        //public static void Get_Next_Day()
        //{
        //    Temperature_Readings = new List<DataPoint>();
        //    Humidity_Readings = new List<DataPoint>();
        //    SetPoint_Readings = new List<DataPoint>();

        //   if(Current_Day < Reading_Upper_Bound)
        //   {
        //        Current_Day = Current_Day.AddDays(1);
        //        Populate_Current_Day();
        //   }

        //   else if(Current_Day == Reading_Upper_Bound)
        //   {
        //        Current_Day = Reading_Upper_Bound;
        //        Populate_Current_Day();
        //    }
        //}

        //public static void Get_Previous_Day()
        //{
        //    Temperature_Readings = new List<DataPoint>();
        //    Humidity_Readings = new List<DataPoint>();
        //    SetPoint_Readings = new List<DataPoint>();

        //    if (Current_Day > Reading_Lower_Bound)
        //    {
        //        Current_Day = Current_Day.AddDays(-1);
        //        Populate_Current_Day();
        //    }

        //    else if(Current_Day == Reading_Lower_Bound)
        //    {
        //        Current_Day = Reading_Lower_Bound;
        //        Populate_Current_Day();
        //    }
        //}

        //public static void Populate_Current_Day()
        //{
        //    foreach (ReadingCell cell in Readings.Where(x => x.Occurence >= Current_Day && x.Occurence < Current_Day.AddDays(1)))
        //    {
        //        Temperature_Readings.Add(new DataPoint
        //        (
        //            DateTimeAxis.ToDouble(cell.Occurence),
        //            cell.Get_Temperature(ReadingCell.ReadingUnitType.Fahrenheit)
        //        ));

        //        Humidity_Readings.Add(new DataPoint
        //        (
        //            DateTimeAxis.ToDouble(cell.Occurence),
        //            cell.Humidity
        //        ));

        //        SetPoint_Readings.Add(new DataPoint
        //        (
        //            DateTimeAxis.ToDouble(cell.Occurence),
        //            cell.Set_Point
        //        ));
        //    }
        //}

        // Used with all settings => 1 week = 7 days
        
        #endregion
    }
}
