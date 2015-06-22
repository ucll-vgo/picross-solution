using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace GUI.Converters
{
    [ContentProperty("ConverterTree")]
    public class CurriedConverter : IMultiValueConverter
    {        
        public IValueConverter ConverterTree { get; set; }

        public object Convert( object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            IValueConverter current = ConverterTree;

            for ( var i = 0; i < values.Length - 1; ++i )
            {
                current = (IValueConverter) current.Convert( values[i], typeof( IValueConverter ), parameter, culture );
            }

            return current.Convert( values[values.Length - 1], targetType, parameter, culture );
        }

        public object[] ConvertBack( object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
