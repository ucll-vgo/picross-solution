using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using PiCross.DataStructures;
using PiCross.Game;
using UIGrid = System.Windows.Controls.Grid;

namespace GUI.Controls
{
    /// <summary>
    /// Interaction logic for PuzzleControl.xaml
    /// </summary>
    public partial class PuzzleControl : UserControl
    {
        public PuzzleControl()
        {
            InitializeComponent();
        }

        public IPuzzleViewModel ViewModel
        {
            get { return (IPuzzleViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register( "ViewModel", typeof( IPuzzleViewModel ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnViewModelChanged( args ) ) );

        private void OnViewModelChanged( DependencyPropertyChangedEventArgs args )
        {
            RemoveAll();
            CreateAll();
        }

        private void RemoveAll()
        {
            this.grid.Children.Clear();
            this.grid.ColumnDefinitions.Clear();
            this.grid.RowDefinitions.Clear();
        }

        private void CreateAll()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            CreateColumnDefinitions();
            CreateRowDefinitions();
            CreateChildren();
        }

        private void CreateColumnDefinitions()
        {
            this.grid.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength( 1, GridUnitType.Star ) } );

            for ( var i = 0; i != ViewModel.ColumnConstraints.Length; ++i )
            {
                this.grid.ColumnDefinitions.Add( new ColumnDefinition() { Width = GridLength.Auto } );
            }
        }

        private void CreateRowDefinitions()
        {
            this.grid.RowDefinitions.Add( new RowDefinition() { Height = new GridLength( 1, GridUnitType.Star ) } );

            for ( var i = 0; i != ViewModel.ColumnConstraints.Length; ++i )
            {
                this.grid.RowDefinitions.Add( new RowDefinition() { Height = GridLength.Auto } );
            }
        }

        private void CreateChildren()
        {
            CreateSquareControls();
        }

        private void CreateSquareControls()
        {
            foreach ( var position in ViewModel.Grid.Squares.AllPositions )
            {
                Debug.WriteLine("Creating child at {0}", position);

                var gridCol = position.X + 1;
                var gridRow = position.Y + 1;
                var squareViewModel = ViewModel.Grid.Squares[position];
                var squareControl = new PuzzleGridSquareControl() { ViewModel = squareViewModel };

                UIGrid.SetColumn( squareControl, gridCol );
                UIGrid.SetRow( squareControl, gridRow );

                this.grid.Children.Add( squareControl );
            }
        }
    }

    public interface IPuzzleViewModel
    {
        IPuzzleGridViewModel Grid { get; }

        ISequence<IPuzzleConstraintsViewModel> ColumnConstraints { get; }

        ISequence<IPuzzleConstraintsViewModel> RowConstraints { get; }
    }

    public interface IPuzzleGridViewModel
    {
        IGrid<IPuzzleGridSquareViewModel> Squares { get; }
    }

    public interface IPuzzleConstraintsViewModel
    {
        ISequence<IPuzzleConstraintsValueViewModel> Values { get; }

        ICell<bool> IsSatisfied { get; }
    }

    public interface IPuzzleConstraintsValueViewModel
    {
        ICell<int> Number { get; }

        ICell<bool> IsSatisfied { get; }
    }
}
