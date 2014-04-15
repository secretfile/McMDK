using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace McMDK.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanToReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type target, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? b = value as bool?;
            if(b == null)
            {
                return false;
            }
            return !b;
        }

        public object ConvertBack(object value, Type target, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
