using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    public class FlowCardStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "开工";
                case 1:
                    return "报工";
                case 2:
                    return "完工";
                case 3:
                    return "被分批";
                default :
                    return "开立";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "开工":
                    return 0;
                case "报工":
                    return 1;
                case "完工":
                    return 2;
                case "被分批":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
