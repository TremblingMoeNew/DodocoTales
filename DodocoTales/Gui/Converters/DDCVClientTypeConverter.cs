using DodocoTales.Library;
using DodocoTales.Library.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DodocoTales.Gui.Converters
{
    public class DDCVClientTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DDCLGameClientType type = (DDCLGameClientType)value;
            return DDCL.GetGameClientTypeName(type);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
