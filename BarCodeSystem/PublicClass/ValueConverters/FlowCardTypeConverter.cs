using System;
using System.Globalization;
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
                    return "分批流转卡";
                case 2:
                    return "转序流转卡";
                case 3:
                    return "返工流转卡";
                case 4:
                    return "无来源流转卡";
                default:
                    return "普通流转卡";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "普通流转卡":
                    return 0;
                case "分批流转卡":
                    return 1;
                case "转序流转卡":
                    return 2;
                case "返工流转卡":
                    return 3;
                case "无来源流转卡":
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
