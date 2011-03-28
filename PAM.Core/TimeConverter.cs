using System;
using System.Globalization;
using System.Windows.Data;

namespace PAM.Core
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeConverter : IValueConverter
    {
        public static TimeSpan AppsTotalTime;

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {

            var userValue = value is TimeSpan ? (TimeSpan)value : new TimeSpan();



            var result = string.Empty;

            if (userValue.Hours > 0)
            {
                result += userValue.Hours + "h ";
            }

            if (userValue.Minutes > 0)
            {
                result += userValue.Minutes + "m ";
            }

            if (userValue.Seconds > 0)
            {
                result += userValue.Seconds + "s";
            }
            if ((int)AppsTotalTime.TotalSeconds > 0)
            {

                var valueInPercentage = (userValue.TotalSeconds * 100) / AppsTotalTime.TotalSeconds;

                var percentageAsString = valueInPercentage < 1 ? "< 1" : valueInPercentage.ToString("0.0");

                result += String.Format(" ({0}%)", percentageAsString);
            }

            return result;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}