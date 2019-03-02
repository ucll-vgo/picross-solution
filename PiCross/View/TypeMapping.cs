using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace GUI
{
    [ContentProperty( "Template" )]
    public class TypeMapping
    {
        public Type Type { get; set; }

        public DataTemplate Template { get; set; }
    }
}
