using System;
using System.Windows.Data;

namespace GUI.Converters
{
    public class BoolConverter : IValueConverter
    {
        public object True { get; set; }

        public object False { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var b = (bool) value;

            if ( b )
            {
                return True;
            }
            else
            {
                return False;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
