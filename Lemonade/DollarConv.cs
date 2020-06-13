using System.Globalization;

namespace Lemonade
{
    public static class DollarConv
    {
        public static string ToDollar(this int cents)
        {
            string tmp = $"${(cents / 100f).ToString(CultureInfo.InvariantCulture)}";
            if (tmp.Length > 1 && tmp[1] == '-')
                tmp = "-$" + tmp.Substring(2);
            if (tmp.Contains('.') && tmp.Split('.')[1].Length < 2)
                tmp += '0';
            return tmp;
        }
    }
}