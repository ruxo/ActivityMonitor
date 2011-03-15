namespace PAM.Core.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    namespace PAM.Core
    {
        [ValueConversion(typeof(int), typeof(string))]
        public class SecondsToTimeConverter : IValueConverter
        {
            public object Convert(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
            {

                var userValue = value is int ? TimeSpan.FromSeconds((int)value): new TimeSpan();



                var result = string.Empty;

                if (userValue.Hours > 0)
                {
                    result += userValue.Hours + " hour";
                    if (userValue.Hours > 1)
                        result += "s";

                    result += " ";
                }

                if (userValue.Minutes > 0)
                {
                    result += userValue.Minutes + " minute";
                    if( userValue.Minutes > 1)
                        result += "s";

                    result += " ";
                }

                if (userValue.Seconds > 0)
                {
                    result += userValue.Seconds + " second";
                    if( userValue.Seconds > 1)
                        result +="s";

                    result += " ";
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
}