﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DodocoTales.Gui.Converters
{
    public class DDCVUidNormalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = "未登录";
            try
            {
                long val = System.Convert.ToInt64(value);
                if (val > 1000000) res = String.Format("{0}", val);
                else if (val >= 10000) res = String.Format("{0} (匿名)", val);
                else if (val >= 0) res = String.Format("{0} (临时)", val);
            }
            catch { }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}