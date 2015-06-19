using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    public class FlowCardTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "普通流转卡";
                case 1:
                    return "返工流传卡";
                case 2:
                    return "分批流转卡";
                case 3:
                default:
                    return "无来源流转卡";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "普通流转卡":
                    return 0;
                case "返工流转卡":
                    return 1;
                case "分批流转卡":
                    return 2;
                case "无来源流转卡":
                default:
                    return 3;
            }
        }
    }
}
