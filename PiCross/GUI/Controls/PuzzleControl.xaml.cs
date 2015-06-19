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
            DependencyProperty.Register( "SquareTemplate", typeof( DataTemplate ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnSquareTemplateChanged( args ) ) );

        private void OnSquareTemplateChanged( DependencyPropertyChangedEventArgs args )
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

        #region PuzzleData

        public IPuzzleData PuzzleData
        {
            get { return (IPuzzleData) GetValue( PuzzleDataProperty ); }
            set { SetValue( PuzzleDataProperty, value ); }
        }

        public static readonly DependencyProperty PuzzleDataProperty =
            DependencyProperty.Register( "PuzzleData", typeof( IPuzzleData ), typeof( PuzzleControl ), new PropertyMetadata( null, ( obj, args ) => ( (PuzzleControl) obj ).OnDataChanged( args ) ) );

        private void OnDataChanged( DependencyPropertyChangedEventArgs args )
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
            ClearColumnDefinitions();
            ClearRowDefinitions();
        }

        private void ClearColumnDefinitions()
        {
            this.grid.ColumnDefinitions.Clear();
        }

        private void ClearRowDefinitions()
        {
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

        private void RecreateColumnDefinitions()
        {
            ClearColumnDefinitions();
            CreateColumnConstraintControls();
        }

        private void RecreateRowDefinitions()
        {
            ClearRowDefinitions();
            CreateRowConstraintControls();
        }

        private void CreateColumnDefinitions()
        {
            Debug.Assert( this.grid.ColumnDefinitions.Count == 0 );

            if ( PuzzleData != null )
            {
                this.grid.ColumnDefinitions.Add( new ColumnDefinition() { Width = GridLength.Auto } );

                for ( var i = 0; i != PuzzleData.ColumnConstraints.Length; ++i )
                {
                    this.grid.ColumnDefinitions.Add( new ColumnDefinition() { Width = GridLength.Auto } );
                }
            }
        }

        private void CreateRowDefinitions()
        {
            Debug.Assert( this.grid.RowDefinitions.Count == 0 );

            if ( PuzzleData != null )
            {
                this.grid.RowDefinitions.Add( new RowDefinition() { Height = GridLength.Auto } );

                for ( var i = 0; i != PuzzleData.ColumnConstraints.Length; ++i )
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
            if ( PuzzleData != null && SquareTemplate != null )
            {
                foreach ( var position in PuzzleData.Grid.AllPositions )
                {
                    var gridCol = position.X + 1;
                    var gridRow = position.Y + 1;
                    var squareData = PuzzleData.Grid[position];
                    var squareControl = (FrameworkElement) SquareTemplate.LoadContent();

                    squareControl.DataContext = squareData;
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
            if ( PuzzleData != null && ColumnConstraintsTemplate != null )
            {
                foreach ( var index in PuzzleData.ColumnConstraints.Indices )
                {
                    var columnIndex = index + 1;
                    var columnConstraintData = PuzzleData.ColumnConstraints[index];
                    var constraintsControl = (FrameworkElement) ColumnConstraintsTemplate.LoadContent();

                    constraintsControl.DataContext = columnConstraintData;
                    UIGrid.SetRow( constraintsControl, 0 );
                    UIGrid.SetColumn( constraintsControl, columnIndex );

                    this.grid.Children.Add( constraintsControl );
                }
            }
        }

        private void CreateRowConstraintControls()
        {
            if ( PuzzleData != null && RowConstraintsTemplate != null )
            {
                foreach ( var index in PuzzleData.RowConstraints.Indices )
                {
                    var rowIndex = index + 1;
                    var rowConstraintData = PuzzleData.RowConstraints[index];
                    var constraintsControl = (FrameworkElement) RowConstraintsTemplate.LoadContent();

                    constraintsControl.DataContext = rowConstraintData;
                    UIGrid.SetRow( constraintsControl, rowIndex );
                    UIGrid.SetColumn( constraintsControl, 0 );

                    this.grid.Children.Add( constraintsControl );
                }
            }
        }

        #endregion
    }

    public interface IPuzzleData
    {
        IGrid<object> Grid { get; }

        ISequence<object> ColumnConstraints { get; }

        ISequence<object> RowConstraints { get; }
    }    
}
