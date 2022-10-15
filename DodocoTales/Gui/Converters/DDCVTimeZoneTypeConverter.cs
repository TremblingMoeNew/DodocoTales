using DodocoTales.Common.Enums;
using DodocoTales.Library;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DodocoTales.Gui.Converters
{
    public class DDCVTimeZoneTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DDCCTimeZone type = (DDCCTimeZone)value;
            return DDCL.GetTimeZoneName(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
