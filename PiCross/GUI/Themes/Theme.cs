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

        public Theme( string name, Uri uri )
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
        private readonly IList<Theme> themes;

        public ThemeManager()
        {
            this.themes = new List<Theme>() { 
                new Theme( "Generic", new Uri( "/Themes/Generic.xaml", UriKind.RelativeOrAbsolute ) ),
                new Theme( "Nature", new Uri( "/Themes/Nature.xaml", UriKind.RelativeOrAbsolute ) ),
                new Theme( "Water", new Uri( "/Themes/Water.xaml", UriKind.RelativeOrAbsolute ) ) 
            };
        }

        public IList<Theme> Themes
        {
            get
            {
                return themes;
            }
        }

        public Theme DefaultTheme
        {
            get
            {
                return new Theme( "Generic", new Uri( "/Themes/Generic.xaml", UriKind.RelativeOrAbsolute ) );
            }
        }
    }
}
