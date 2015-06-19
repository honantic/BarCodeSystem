using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BarCodeSystem.PublicClass.ValidationRules
{
    public class QualifiedAmountRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int a = 0;
            if (int.TryParse(value.ToString(), out a))
            {
                if (a<0)
                {
                    return new ValidationResult(false, "合格数不能小于0");
                }
            }
            return new ValidationResult(true, null);
            //throw new NotImplementedException();
        }
    }
}
