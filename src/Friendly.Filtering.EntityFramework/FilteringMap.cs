using Friendly.Core;

namespace Friendly.Filtering.EntityFramework
{
    public class FilteringMap
    {
        public string From { get; set; }
        public string To { get; set; }
        public bool Pad { get; set; }

        public string FromFormatted => Pad ? From.Replace(".", "/").WrapWithChar() : From.Replace(".", "/");
        public string ToFormatted => Pad ? To.Replace(".", "/").WrapWithChar() : To.Replace(".", "/");
    }
}