using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BarCodeSystem.PublicClass.ValueConverters
{
    public class QualifiedAmountConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int amount = 0;
            try
            {
                List<int> para = new List<int>();
                foreach (object item in values)
                {
                    para.Add(System.Convert.ToInt32(item.ToString()));
                }
                amount = para.ElementAt(0) - para.ElementAt(1) - para.ElementAt(2);
                if (amount < 0)
                {
                    MessageBox.Show("合格数量不能小于0！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "0";
                }
                else
                {
                    return amount.ToString();
                }
            }
            catch (Exception)
            {
                return "0";
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
