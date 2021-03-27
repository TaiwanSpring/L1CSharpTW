namespace System.Extensions
{
    static class StringExtensions
    {
        public static int ParseInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            int.TryParse(str, out int i);
            return i;
        }
    }
}