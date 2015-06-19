using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    public class ReportWayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "流水线报工";
                case 1:
                default:
                    return "离散报工";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "流水线报工":
                    return 0;
                case "离散报工":
                default:
                    return 1;
            }
        }
    }
}
