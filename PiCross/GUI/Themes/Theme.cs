using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Themes
{
    public class Theme
    {
        private readonly string name;

        private readonly Uri uri;

        public Theme(string name, Uri uri)
        {
            this.name = name;
            this.uri = uri;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Uri Uri
        {
            get
            {
                return uri;
            }
        }
    }

    public class ThemeManager
    {
        public IEnumerable<Theme> Themes
        {
            get
            {
                yield return new Theme( "Generic", new Uri("/Themes/Generic.xaml", UriKind.RelativeOrAbsolute) );
                yield return new Theme( "Nature", new Uri( "/Themes/Nature.xaml", UriKind.RelativeOrAbsolute ) );
                yield return new Theme( "Water", new Uri( "/Themes/Water.xaml", UriKind.RelativeOrAbsolute ) );
            }
        }
    }
}
