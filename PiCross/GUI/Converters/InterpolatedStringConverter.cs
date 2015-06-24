using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GUI.Converters
{
    public class InterpolatedStringConverter : IValueConverter
    {
        public string String { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return Regex.Replace(String, @"\[\[(\w+?)\]\]", m => GetPropertyValue( value, m.Groups[1].Value ).ToString() );
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty( propertyName ).GetValue( obj );
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
