using DodocoTales.Library;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DodocoTales.Gui.Converter
{
    public class DDCVUidButtonDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = "-----未登录-----";
            try
            {
                long val = System.Convert.ToInt64(value);
                if (val > 0) res = String.Format("UID:{0}****{1}", val / 10000000, val % 1000);
            }
            catch (Exception)
            {

            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
