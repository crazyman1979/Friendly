namespace Friendly.Core
{
    public static class ConversionExtenstions
    {
        public static int ToInt(this string val)
        {
            return !string.IsNullOrEmpty(val) ? int.Parse(val) : 0;
        }
    }
}