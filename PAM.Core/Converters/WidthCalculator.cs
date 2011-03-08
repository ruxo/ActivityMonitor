using System;
using PAM.Core.Implementation.Monitor;

namespace PAM.Core.Converters
{
    public static class WidthCalculator
    {
        public const int MaxControlWidth = 300;
        public const int MinControlWidthSwitch = 100;

        public static double Calculate(TimeSpan value)
        {
            const int controlWidth = MaxControlWidth;
            var maxInSeconds = AppUpdater.GetMaxValue;
            var newValue = (int)(value.TotalSeconds);
            return (newValue / maxInSeconds) * controlWidth;
        }

    }
}