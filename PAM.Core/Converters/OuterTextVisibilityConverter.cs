using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PAM.Core.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(Visibility))]
    public class OuterTextVisibilityConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var controlWidth = WidthCalculator.Calculate((TimeSpan)value);
            return controlWidth > WidthCalculator.MinControlWidthSwitch ? Visibility.Collapsed : Visibility.Visible;


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