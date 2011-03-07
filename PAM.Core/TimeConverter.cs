using System;
using System.Globalization;
using System.Windows.Data;

namespace PAM.Core
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {

            var userValue = value is TimeSpan ? (TimeSpan)value : new TimeSpan();



            var result = string.Empty;

            if (userValue.Hours > 0) {
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