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



        public DataTemplate SquareTemplate
        {
            get { return (DataTemplate) GetValue( SquareTemplateProperty ); }
            set { SetValue( SquareTemplateProperty, value ); }
        }

        public static readonly DependencyProperty SquareTemplateProperty =
            DependencyProperty.Register( "SquareTemplate", typeof( DataTemplate ), typeof( PuzzleControl ), new PropertyMetadata( null, (obj, args) => ((PuzzleControl) obj).OnSquareTemplateChanged(args) ) );

        private void OnSquareTemplateChanged(DependencyPropertyChangedEventArgs args)
        {
            ClearChildren();
            CreateChildren();
        }

        public IPuzzleViewModel ViewModel
        {
            get { return (IPuzzleViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register( "ViewModel", typeof( IPuzzleViewModel ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnViewModelChanged( args ) ) );

        private void OnViewModelChanged( DependencyPropertyChangedEventArgs args )
        {
            ClearAll();
            CreateAll();
        }

        private void ClearAll()
        {
            ClearChildren();
            ClearGridLayout();
        }

        private void ClearChildren()
        {
            this.grid.Children.Clear();
        }
        
        private void ClearGridLayout()
        {
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
            if ( ViewModel != null )
            {
                this.grid.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength( 1, GridUnitType.Star ) } );

                for ( var i = 0; i != ViewModel.ColumnConstraints.Length; ++i )
                {
                    this.grid.ColumnDefinitions.Add( new ColumnDefinition() { Width = GridLength.Auto } );
                }
            }
        }

        private void CreateRowDefinitions()
        {
            if ( ViewModel != null )
            {
                this.grid.RowDefinitions.Add( new RowDefinition() { Height = new GridLength( 1, GridUnitType.Star ) } );

                for ( var i = 0; i != ViewModel.ColumnConstraints.Length; ++i )
                {
                    this.grid.RowDefinitions.Add( new RowDefinition() { Height = GridLength.Auto } );
                }
            }
        }

        private void CreateChildren()
        {
            if ( ViewModel != null )
            {
                foreach ( var position in ViewModel.Grid.Squares.AllPositions )
                {
                    var gridCol = position.X + 1;
                    var gridRow = position.Y + 1;
                    var squareViewModel = ViewModel.Grid.Squares[position];
                    var squareControl = (FrameworkElement) SquareTemplate.LoadContent();

                    squareControl.DataContext = squareViewModel;
                    UIGrid.SetColumn( squareControl, gridCol );
                    UIGrid.SetRow( squareControl, gridRow );

                    this.grid.Children.Add( squareControl );
                }
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
