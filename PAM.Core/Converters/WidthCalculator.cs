using System;
using PAM.Core.Implementation.Monitor;

namespace PAM.Core.Converters
{
    public static class WidthCalculator
    {
        public static double MaxControlWidth = 300;
        public const int MinControlWidthSwitch = 80;

        public static double Calculate(TimeSpan value)
        {
            var controlWidth = MaxControlWidth - 280;
            var maxInSeconds = AppUpdater.GetMaxValue;
            var newValue = (int)(value.TotalSeconds);
            return (newValue / maxInSeconds) * controlWidth;
        }

    }
}