using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    /// <summary>
    /// 将数据库的true,false转换成是、否
    /// </summary>
    public class TrueOrFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((bool)value)
            {
                case false:
                    return "否";
                case true:
                    return "是";
                default:
                    return "否";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "是":
                    return true;
                case "否":
                    return false;
                default:
                    return false;
            }
        }
    }
}
