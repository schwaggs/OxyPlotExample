using System;
using System.Collections.Generic;
using System.Text;

namespace OxyPlotExamples
{
    public static class PlotTouch
    {
        #region Enumerations 

        public enum IntervalType
        {
            Hour,
            Week,
            Month
        }

        public enum Orientation
        {
            Horizontal,
            Vertical,
            None
        }

        #endregion Enumerations


        #region Drawing Information

        // Set during initialization and used for calculating the workable area
        public static double Right_Margin { get; set; }
        public static double Left_Margin { get; set; }
        public static double Left_Margin_Offset { get; set; }
        public static Orientation Current_Orientation { get; set; }

        #endregion Drawing Information


        #region Boundaries

        /// <summary>
        /// Plot specific information used for boudning and calculations
        /// </summary>
        private static readonly TimeSpan Swipe_Acceleration_Time = new TimeSpan(0, 0, 0, 0, 150);

        private static readonly int Hour_Step_Max = 25; // Hours
        private static readonly int Hour_Step_Min = 0;

        private static readonly int Week_Step_Max = 8;  // Days
        private static readonly int Week_Step_Min = 0;

        private static int Month_Step_Max = 31;              // Days
        private static readonly int Month_Step_Min = 0;

        #endregion Boundaries


        #region Positions

        public static Position Initial_Touch { get; set; }
        public static Position Running_Touch { get; set; }
        public static Position Final_Touch { get; set; }


        #endregion


        #region Screen Changed
        public static double Current_Screen_Width { get; set; }
        private static double Current_Plot_Width
        {
            get
            {
                return Current_Screen_Width - (Left_Margin + Left_Margin_Offset + Right_Margin);
            }
        }

        #endregion


        #region Methods

        public static void Init()
        {
            Current_Orientation = Orientation.None;

            Initial_Touch = new Position
            {
                X_Location = 0,
                Start_Time = DateTime.MinValue,
                X_Change = 0
            };

            Running_Touch = new Position
            {
                X_Location = 0,
                Start_Time = DateTime.MinValue,
                X_Change = 0
            };

            Final_Touch = new Position
            {
                X_Location = 0,
                Start_Time = DateTime.MinValue,
                X_Change = 0
            };

            Panning = false;

            Current_Interval = IntervalType.Hour;
        }

        public static void Set_Margins(double LeftMargin, double LeftMarginOffset, double RightMargin)
        {
            Left_Margin = LeftMargin;
            Left_Margin_Offset = LeftMarginOffset;
            Right_Margin = RightMargin;
        }

        #endregion


        #region Runtime Information

        public static bool Panning { get; set; }
        private static double Current_Tick_Max { get; set; }
        private static double Current_Tick_Min { get; set; }
        private static IntervalType _Current_Interval { get; set; }

        public static IntervalType Current_Interval
        {

            get
            {
                return _Current_Interval;
            }

            set
            {
                _Current_Interval = value;

                switch(value)
                {
                    case IntervalType.Hour:

                        Current_Tick_Max = Hour_Step_Max;
                        Current_Tick_Min = Hour_Step_Min;

                        break;

                    case IntervalType.Week:

                        Current_Tick_Max = Week_Step_Max;
                        Current_Tick_Min = Week_Step_Min;

                        break;

                    case IntervalType.Month:

                        Current_Tick_Max = Month_Step_Max;
                        Current_Tick_Min = Month_Step_Min;

                        break;
                }
            }
        }

        private static double Current_Step_Size
        {
            get
            {
                return Current_Plot_Width / Current_Tick_Max;
            }
        }

        private static double Current_Plot_Location
        {
            get
            {
                if (!Panning)
                {
                    return Initial_Touch.X_Location - (Left_Margin + Left_Margin_Offset);
                }

                else
                {
                    return Running_Touch.X_Location - (Left_Margin + Left_Margin_Offset);
                }
            }
        }

        public static int Index
        {
            get
            {
                double temp = Math.Round(Current_Plot_Location / Current_Step_Size, MidpointRounding.AwayFromZero);

                if(temp <= Current_Tick_Min)
                {
                    return (int)Current_Tick_Min;
                }

                else if(temp >= Current_Tick_Max)
                {
                    return (int)(Current_Tick_Max - 1);
                }

                else
                {
                    return (int)(temp - 1);
                }
            }
        }

        public static bool Is_Swipe()
        {
            if (Final_Touch.Start_Time - Initial_Touch.Start_Time < Swipe_Acceleration_Time)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        #endregion

        public class Position
        {
            public double X_Location { get; set; }
            public DateTime Start_Time { get; set; }
            public double X_Change { get; set; }
        }
    }
}
