using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BarCodeSystem
{
    public class ValueConverters 
    {

        /// <summary>
        /// 料品编码的转换器
        /// </summary>
        public class TechRoute_ItemCodeConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                TechRouteLists trl = (TechRouteLists)value;
                return trl.TR_ItemCode ?? "";
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
