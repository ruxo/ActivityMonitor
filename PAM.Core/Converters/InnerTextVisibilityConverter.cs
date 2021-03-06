using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PAM.Core.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(Visibility))]
    public class InnerTextVisibilityConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {

            var controlWidth = WidthCalculator.Calculate((TimeSpan)value);
            return controlWidth < WidthCalculator.MinControlWidthSwitch ? Visibility.Hidden : Visibility.Visible;

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