using System;
using System.Collections.Generic;
using System.Text;

namespace OxyPlotExamples
{
    public class ReadingCell
    {
        private int _Set_Point { get; set; }
        private int _Humidity { get; set; }
        private double _Temperature { get; set; }
        private DateTime _Occurence { get; set; }

        public int Set_Point
        {
            get
            {
                return _Set_Point;
            }
        }

        public int Humidity
        {
            get
            {
                return _Humidity;
            }
        }

        public DateTime Occurence
        {
            get
            {
                return _Occurence;
            }
        }

        public ReadingCell(RawReading reading)
        {
            _Set_Point = Convert.ToInt32(reading.Set_Point);         // In percentage
            _Humidity = Convert.ToInt32(reading.Humidity);           // In percentage
            _Temperature = Convert.ToDouble(reading.Temperature);    // In degrees Fahrenheit
            _Occurence = Convert.ToDateTime(reading.Occurence);      // Some datetime format string
        }

        public double Get_Temperature(ReadingUnitType unit)
        {
            switch (unit)
            {
                case ReadingUnitType.Fahrenheit:
                    return _Temperature;

                case ReadingUnitType.Celcius:
                    return (_Temperature - 32) / 1.8;

                default:
                    return 0;
            }
        }

        public enum ReadingUnitType
        {
            Fahrenheit,
            Celcius
        }
    }
}
