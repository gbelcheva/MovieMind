namespace Crawler
{
    public static class ParseExtensions
    {
        public static int? ToInt(this string val)
        {
            int result;
            if (int.TryParse(val, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static double? ToDouble(this string val)
        {
            double result;
            if (double.TryParse(val, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
