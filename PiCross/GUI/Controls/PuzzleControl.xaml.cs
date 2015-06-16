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
using PiCross.Cells;
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

        #region SquareTemplate

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

        #endregion

        #region ColumnConstraintsTemplate

        public DataTemplate ColumnConstraintsTemplate
        {
            get { return (DataTemplate) GetValue( ColumnConstraintsTemplateProperty ); }
            set { SetValue( ColumnConstraintsTemplateProperty, value ); }
        }

        public static readonly DependencyProperty ColumnConstraintsTemplateProperty =
            DependencyProperty.Register( "ColumnConstraintsTemplate", typeof( DataTemplate ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnColumnConstraintsTemplateChanged( args ) ) );

        private void OnColumnConstraintsTemplateChanged( DependencyPropertyChangedEventArgs args )
        {
            RecreateChildren();
        }

        #endregion

        #region RowConstraintsTemplate

        public DataTemplate RowConstraintsTemplate
        {
            get { return (DataTemplate) GetValue( RowConstraintsTemplateProperty ); }
            set { SetValue( RowConstraintsTemplateProperty, value ); }
        }

        public static readonly DependencyProperty RowConstraintsTemplateProperty =
            DependencyProperty.Register( "RowConstraintsTemplate", typeof( DataTemplate ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnRowConstraintsTemplateChanged( args ) ) );
                
        private void OnRowConstraintsTemplateChanged( DependencyPropertyChangedEventArgs args )
        {
            RecreateChildren();
        }

        #endregion

        #region ViewModel

        public IPuzzleViewModel ViewModel
        {
            get { return (IPuzzleViewModel) GetValue( ViewModelProperty ); }
            set { SetValue( ViewModelProperty, value ); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register( "ViewModel", typeof( IPuzzleViewModel ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnViewModelChanged( args ) ) );

        private void OnViewModelChanged( DependencyPropertyChangedEventArgs args )
        {
            RecreateAll();
        }

        #endregion

        #region Children

        private void RecreateAll()
        {
            ClearAll();
            CreateAll();
        }

        private void RecreateChildren()
        {
            ClearChildren();
            CreateChildren();
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
            Debug.Assert( this.grid.ColumnDefinitions.Count == 0 );

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
            Debug.Assert( this.grid.RowDefinitions.Count == 0 );

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
            Debug.Assert( this.grid.Children.Count == 0 );

            CreateSquareControls();
            CreateConstraintControls();
        }

        private void CreateSquareControls()
        {
            if ( ViewModel != null && SquareTemplate != null )
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

        private void CreateConstraintControls()
        {
            CreateColumnConstraintControls();
            CreateRowConstraintControls();            
        }

        private void CreateColumnConstraintControls()
        {
            if ( ViewModel != null && ColumnConstraintsTemplate != null )
            {
                foreach ( var index in ViewModel.ColumnConstraints.Indices )
                {
                    var columnIndex = index + 1;
                    var columnConstraintViewModel = ViewModel.ColumnConstraints[index];
                    var constraintsControl = (FrameworkElement) ColumnConstraintsTemplate.LoadContent();

                    constraintsControl.DataContext = columnConstraintViewModel;
                    UIGrid.SetRow( constraintsControl, 0 );
                    UIGrid.SetColumn( constraintsControl, columnIndex );

                    this.grid.Children.Add( constraintsControl );
                }
            }
        }
        
        private void CreateRowConstraintControls()
        {
            if ( ViewModel != null && RowConstraintsTemplate != null )
            {
                foreach ( var index in ViewModel.RowConstraints.Indices )
                {
                    var rowIndex = index + 1;
                    var rowConstraintViewModel = ViewModel.RowConstraints[index];
                    var constraintsControl = (FrameworkElement) RowConstraintsTemplate.LoadContent();

                    constraintsControl.DataContext = rowConstraintViewModel;
                    UIGrid.SetRow( constraintsControl, rowIndex );
                    UIGrid.SetColumn( constraintsControl, 0 );

                    this.grid.Children.Add( constraintsControl );
                }
            }
        }

        #endregion
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

    public interface IPuzzleConstraintsValueViewModel
    {
        Cell<int> Number { get; }

        Cell<bool> IsSatisfied { get; }
    }
}
