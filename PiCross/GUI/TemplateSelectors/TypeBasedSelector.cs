using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI.TemplateSelectors
{
    [ContentProperty( "Items" )]
    public class TypeBasedSelector : DataTemplateSelector
    {
        private Collection<TypeMapping> pairs;

        public TypeBasedSelector()
        {
            pairs = new Collection<TypeMapping>();
        }

        public override System.Windows.DataTemplate SelectTemplate( object item, System.Windows.DependencyObject container )
        {
            var type = item.GetType();

            foreach ( var pair in pairs )
            {
                if ( type.Equals( pair.Type ) )
                {
                    return pair.Template;
                }
            }

            throw new ArgumentOutOfRangeException( "item" );
        }

        public Collection<TypeMapping> Items
        {
            get
            {
                return pairs;
            }
        }
    }    
}
