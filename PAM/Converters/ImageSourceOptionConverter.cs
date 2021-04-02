using System.Windows.Data;
using System.Windows.Media;
using LanguageExt;
using PAM.Core.Converters;

namespace PAM.Converters
{
    [ValueConversion(typeof(Option<ImageSource>), typeof(ImageSource))]
    public sealed class ImageSourceOptionConverter : OptionConverter<ImageSource>
    {
        public static readonly ImageSourceOptionConverter Default = new();
    }
}