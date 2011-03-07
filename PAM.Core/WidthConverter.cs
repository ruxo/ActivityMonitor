using System;
using System.Globalization;
using System.Windows.Data;
using PAM.Core.Implementation.Monitor;

namespace PAM.Core
{
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            const int controlWidth = 300;
            var maxInSeconds = AppUpdater.GetMaxValue;
            var newValue = (int) (((TimeSpan) value).TotalSeconds);

            return (newValue/maxInSeconds)*controlWidth;

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