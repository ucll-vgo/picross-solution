using System;
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
using Cells;
using PiCross.DataStructures;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for GridViewControl.xaml
    /// </summary>
    public partial class GridViewControl : UserControl
    {
        public GridViewControl()
        {
            InitializeComponent();
        }

        #region Grid

        public IGrid<object> Grid
        {
            get { return (IGrid<object>) GetValue( GridProperty ); }
            set { SetValue( GridProperty, value ); }
        }

        public static readonly DependencyProperty GridProperty =
            DependencyProperty.Register( "Grid", typeof( IGrid<object> ), typeof( GridViewControl ), new PropertyMetadata( null ) );

        #endregion

        #region ItemTemplate

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue( ItemTemplateProperty ); }
            set { SetValue( ItemTemplateProperty, value ); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register( "ItemTemplate", typeof( DataTemplate ), typeof( GridViewControl ), new PropertyMetadata( null ) );

        #endregion
    }
}
