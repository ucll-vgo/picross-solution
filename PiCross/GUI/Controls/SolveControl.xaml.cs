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
using GUI.ViewModels;
using DataStructures;
using PiCross;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for SolveControl.xaml
    /// </summary>
    public partial class SolveControl : UserControl
    {
        public SolveControl()
        {
            InitializeComponent();
        }

        private PlayViewModel ViewModel
        {
            get
            {
                return (PlayViewModel) DataContext;
            }
        }

        #region Square Selection Logic

        private void Square_MouseDown( object sender, MouseButtonEventArgs e )
        {
            var position = ExtractPosition( sender );

            ViewModel.StartSelection( position, e.LeftButton == MouseButtonState.Pressed );
        }

        private void Square_MouseUp( object sender, MouseButtonEventArgs e )
        {
            ViewModel.EndSelection();
        }

        private void Square_MouseEnter( object sender, MouseEventArgs e )
        {
            var position = ExtractPosition( sender ) ;

            ViewModel.DragSelection( position );
            ViewModel.ActivatedSquare.Value = position;
        }

        private Vector2D ExtractPosition( object sender )
        {
            var control = (FrameworkElement) sender;

            return (Vector2D) control.Tag;
        }

        #endregion
    }
}
