using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    public class QualityIssueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (System.Convert.ToInt32(value))
            {
                case 1:
                    return "料废";
                case 2:
                    return "返工";
                default:
                    return "责废";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "料废":
                    return 1;
                case "返工":
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
