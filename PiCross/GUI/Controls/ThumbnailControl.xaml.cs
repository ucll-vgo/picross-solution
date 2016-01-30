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
    /// Interaction logic for ThumbnailControl.xaml
    /// </summary>
    public partial class ThumbnailControl : UserControl
    {
        public ThumbnailControl()
        {
            InitializeComponent();
        }

        #region Grid

        public IGrid<Cell<bool>> Grid
        {
            get { return (IGrid<Cell<bool>>) GetValue( GridProperty ); }
            set { SetValue( GridProperty, value ); }
        }

        public static readonly DependencyProperty GridProperty =
            DependencyProperty.Register( "Grid", typeof( IGrid<Cell<bool>> ), typeof( ThumbnailControl ), new PropertyMetadata( null ) );

        #endregion

        #region PixelSize

        public double PixelSize
        {
            get { return (double) GetValue( PixelSizeProperty ); }
            set { SetValue( PixelSizeProperty, value ); }
        }

        public static readonly DependencyProperty PixelSizeProperty =
            DependencyProperty.Register( "PixelSize", typeof( double ), typeof( ThumbnailControl ), new PropertyMetadata( 2.0 ) );

        #endregion
    }
}
