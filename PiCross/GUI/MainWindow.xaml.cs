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
                "..x..",
                "..x..",
                "xxxxx",
                "..x..",
                "..x.."
                );

            var playGrid = editorGrid.CreatePlayGrid();
            playGrid.Squares.Overwrite( editorGrid.Squares );

            this.DataContext = new GridViewModel( new Puzzle( playGrid ) );
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
            this.grid = new GridViewModel( puzzle );
            // this.columnConstraints = puzzle.
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

        public GridSquareViewModel(IPuzzleSquare square)
        {
            this.square = square;
        }

        public ICell<Square> Contents
        {
            get
            {
                return square.Contents;
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
