using System.Windows.Data;
using LanguageExt;
using PAM.Core.Converters;
using PAM.Core.Tracker;

namespace PAM.Converters
{
    [ValueConversion(typeof(Option<ApplicationInfo>), typeof(ApplicationInfo))]
    public sealed class ApplicationInfoOptionConverter : OptionConverter<ApplicationInfo>
    {
        public static readonly ApplicationInfoOptionConverter Default = new();
    }

}