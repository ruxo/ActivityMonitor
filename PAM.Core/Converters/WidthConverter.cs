using System;
using System.Globalization;
using System.Windows.Data;

namespace PAM.Core.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {


            return WidthCalculator.Calculate((TimeSpan)value);

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