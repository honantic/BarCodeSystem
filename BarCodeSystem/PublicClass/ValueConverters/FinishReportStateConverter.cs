using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    public class FinishReportStateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "未审核";
                case 1:
                    return "已审核";
                default:
                    return "未审核";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "未审核":
                    return 0;
                case "已审核":
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
