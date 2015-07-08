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
using GUI.ViewModels.PlayMode;
using PiCross.DataStructures;
using PiCross.Game;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for SolveControl.xaml
    /// </summary>
    public partial class SolveControl : UserControl
    {
        private Square newContents;

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

        private void Square_MouseDown( object sender, MouseButtonEventArgs e )
        {
            var position = ExtractPosition( sender );

            var oldContents = ViewModel.Grid.Squares[position].Contents.Value;

            if ( e.LeftButton == MouseButtonState.Pressed )
            {
                newContents = oldContents != Square.FILLED ? Square.FILLED : Square.UNKNOWN;
            }
            else
            {
                newContents = oldContents != Square.EMPTY ? Square.EMPTY : Square.UNKNOWN;
            }

            ViewModel.SelectionStart = position;
            ViewModel.SelectionEnd = position;
        }

        private void Square_MouseUp( object sender, MouseButtonEventArgs e )
        {
            var vm = ViewModel;            

            foreach ( var position in vm.Grid.Squares.AllPositions )
            {
                vm.Grid.Squares[position].SetContentsIfSelected( newContents );
            }            

            vm.SelectionStart = null;
            vm.SelectionEnd = null;
        }

        private void Square_MouseEnter( object sender, MouseEventArgs e )
        {
            ViewModel.SelectionEnd = ExtractPosition( sender );
        }

        private Vector2D ExtractPosition( object sender )
        {
            var control = (FrameworkElement) sender;

            return (Vector2D) control.Tag;
        }
    }
}
