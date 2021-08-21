using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DodocoTales.Gui.Converter
{
    public class DDCVUidLoginCheckConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                long first = -1;
                foreach(var val in values)
                {
                    long vi = System.Convert.ToInt64(val);
                    if (first == -1) first = vi;
                    if (first != vi) return false;
                } 
            }
            catch (Exception)
            {
                return false;
            }

            return true;

            //throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
