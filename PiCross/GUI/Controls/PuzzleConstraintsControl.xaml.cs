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

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for PuzzleConstraintsControl.xaml
    /// </summary>
    public partial class PuzzleConstraintsControl : UserControl
    {
        public PuzzleConstraintsControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty OrientationProperty =
            StackPanel.OrientationProperty.AddOwner( typeof( PuzzleConstraintsControl ) );

        public Orientation Orientation
        {
            get { return (Orientation) GetValue( OrientationProperty ); }
            set { SetValue( OrientationProperty, value ); }
        }
    }
}
