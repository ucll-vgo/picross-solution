using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GUI.Converters
{
    [ContentProperty("Subconverter")]
    public class TemplateLoaderConverter : IValueConverter
    {
        public IValueConverter Subconverter { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var template = (DataTemplate) Subconverter.Convert( value, typeof( DataTemplate ), parameter, culture );

            return template.LoadContent();
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
