using System.Windows.Data;
using LanguageExt;
using PAM.Core.Converters;

namespace PAM.Converters
{
    [ValueConversion(typeof(Option<string>), typeof(string))]
    public sealed class StringOptionConverter : OptionConverter<string>
    {
        public static readonly StringOptionConverter Default = new();
    }
}