using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OxyPlotExamples
{
    public static class FormatStrings
    {
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
            Currency,
            Deciaml,
            Exponential,
            FixedPoint,
            General,
            Number,
            Percent,
            RoundTrip,
            Hexadecimal,
            YearMonthDayDash,
            MonthDayYearSlash,
            ShortMonthDaySlash
        }

        public static string OutputFormatType2String(OutputFormatType output)
        {
            switch (output)
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

                case OutputFormatType.Currency:
                    return "C";

                case OutputFormatType.Deciaml:
                    return "D";

                case OutputFormatType.Exponential:
                    return "E";

                case OutputFormatType.FixedPoint:
                    return "F";

                case OutputFormatType.General:
                    return "G";

                case OutputFormatType.Hexadecimal:
                    return "X";

                case OutputFormatType.Number:
                    return "N";

                case OutputFormatType.Percent:
                    return "P";

                case OutputFormatType.RoundTrip:
                    return "R";

                default:
                    return string.Empty;
            }
        }

    }
}
