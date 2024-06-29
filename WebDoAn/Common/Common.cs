namespace WebDoAn.Common
{
    public class Common
    {
        public static string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumberic(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            string thapPhan = "";
            for(int i = 0;i < SoSauDauPhay;i++)
            {
                thapPhan += "#";
            }
            if(thapPhan.Length > 0) thapPhan = "." + thapPhan;
            string snumberformat = string.Format("0:#,##0{0}", thapPhan);
            str = String.Format("{" + snumberformat + "}", GT);

            return str;

        }

        private static bool IsNumberic(object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }
    }
}
