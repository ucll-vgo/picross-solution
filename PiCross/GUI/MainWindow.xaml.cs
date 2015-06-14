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
using PiCross.DataStructures;
using PiCross.Facade.Solving;
using PiCross.Game;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var editorGrid = EditorGrid.FromStrings(
                ".xxxxxxxx.",
                "x........x",
                "x.x......x",
                "x.xxxxxx.x",
                "x......x.x",
                "x......x.x",
                "x.xxxxxx.x",
                "x.x......x",
                "x........x",
                ".xxxxxxxx."
                );

            var playGrid = editorGrid.CreatePlayGrid();
            playGrid.Squares.Overwrite( editorGrid.Squares );

            this.DataContext = new PuzzleViewModel( new Puzzle( playGrid ) );
        }
    }

    public class PuzzleViewModel
    {
        private readonly IPuzzle puzzle;

        private readonly GridViewModel grid;

        private readonly ISequence<ConstraintsViewModel> columnConstraints;

        private readonly ISequence<ConstraintsViewModel> rowConstraints;

        public PuzzleViewModel(IPuzzle puzzle)
        {
            this.puzzle = puzzle;
            this.grid = new GridViewModel( puzzle );
            this.columnConstraints = puzzle.ColumnConstraints.Items.Select( columnConstraints => new ConstraintsViewModel( columnConstraints ) ).ToSequence();
            this.rowConstraints = puzzle.RowConstraints.Items.Select( rowConstraints => new ConstraintsViewModel( rowConstraints ) ).ToSequence();
        }

        public GridViewModel Grid
        {
            get
            {
                return grid;
            }
        }

        public ISequence<ConstraintsViewModel> ColumnConstraints
        {
            get { return columnConstraints; }
        }

        public ISequence<ConstraintsViewModel> RowConstraints
        {
            get { return rowConstraints; }
        }
    }

    public class GridViewModel
    {
        private readonly IPuzzle puzzle;

        public GridViewModel( IPuzzle puzzle )
        {
            this.puzzle = puzzle;
        }

        public IEnumerable<IEnumerable<GridSquareViewModel>> Rows
        {
            get
            {
                return from y in Enumerable.Range( 0, puzzle.Height )
                       select ( from x in Enumerable.Range( 0, puzzle.Width )
                                let position = new Vector2D( x, y )
                                select new GridSquareViewModel( puzzle[position] ) );
            }
        }
    }

    public class GridSquareViewModel
    {
        private readonly IPuzzleSquare square;

        private readonly ICommand toggle;
        
        public GridSquareViewModel(IPuzzleSquare square)
        {
            this.square = square;
            this.toggle = new ToggleCommand( square.Contents );
        }

        public ICell<Square> Contents
        {
            get
            {
                return square.Contents;
            }
        }

        public ICommand Toggle
        {
            get
            {
                return toggle;
            }
        }

        private class ToggleCommand : ICommand
        {
            private readonly ICell<Square> square;

            public ToggleCommand(ICell<Square> square)
            {
                this.square = square;
            }

            public bool CanExecute( object parameter )
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute( object parameter )
            {
                var contents = square.Value;
                Square newContents;

                if ( contents == Square.EMPTY )
                {
                    newContents = Square.FILLED;
                }
                else if ( contents == Square.FILLED )
                {
                    newContents = Square.UNKNOWN;
                }
                else
                {
                    newContents = Square.EMPTY;
                }

                square.Value = newContents;
            }
        }
    }

    public class ConstraintsViewModel
    {
        private readonly IPuzzleConstraints constraints;

        public ConstraintsViewModel(IPuzzleConstraints constraints)
        {
            this.constraints = constraints;
        }

        public IEnumerable<ConstraintsValueViewModel> Values
        {
            get
            {
                return constraints.Values.Items.Select( value => new ConstraintsValueViewModel( value ) );
            }
        }
    }

    public class ConstraintsValueViewModel
    {
        private readonly IPuzzleConstraintsValue value;

        public ConstraintsValueViewModel(IPuzzleConstraintsValue value)
        {
            this.value = value;
        }

        public ICell<bool> IsSatisfied
        {
            get
            {
                return this.value.IsSatisfied;
            }
        }

        public ICell<int> Number
        {
            get
            {
                return Cell.Create( this.value.Value );
            }
        }        
    }
}
