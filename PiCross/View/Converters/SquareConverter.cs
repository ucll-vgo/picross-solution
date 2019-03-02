using System;
using System.Windows.Data;
using PiCross;

namespace GUI.Converters
{
    public class SquareConverter : IValueConverter
    {
        public object Unknown { get; set; }

        public object Empty { get; set; }

        public object Filled { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var square = (Square) value;

            if ( square == Square.UNKNOWN )
            {
                return Unknown;
            }
            else if ( square == Square.EMPTY )
            {
                return Empty;
            }
            else if ( square == Square.FILLED )
            {
                return Filled;
            }
            else
            {
                throw new ArgumentNullException( "null not supported" );
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
