using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for GeneralizedLibraryView.xaml
    /// </summary>
    public partial class GeneralizedLibraryView : UserControl
    {
        public GeneralizedLibraryView()
        {
            InitializeComponent();
        }
        
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue( ItemTemplateProperty ); }
            set { SetValue( ItemTemplateProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register( "ItemTemplate", typeof( DataTemplate ), typeof( GeneralizedLibraryView ), new PropertyMetadata( null ) );

        public DataTemplate GroupHeaderTemplate
        {
            get { return (DataTemplate) GetValue( GroupHeaderTemplateProperty ); }
            set { SetValue( GroupHeaderTemplateProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupHeaderTemplateProperty =
            DependencyProperty.Register( "GroupHeaderTemplate", typeof( DataTemplate ), typeof( GeneralizedLibraryView ), new PropertyMetadata( null ) );

        public IEnumerable Groups
        {
            get { return (IEnumerable) GetValue( GroupsProperty ); }
            set { SetValue( GroupsProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register( "Groups", typeof( IEnumerable ), typeof( GeneralizedLibraryView ), new PropertyMetadata( null ) );
    }
}
