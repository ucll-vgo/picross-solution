using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Playing;

namespace GUI.ViewModels.PlayMode
{
    public class GridViewModel
    {
        private readonly IPlayablePuzzle puzzle;

        private readonly IGrid<GridSquareViewModel> squares;

        public GridViewModel( IPlayablePuzzle puzzle, Cell<Vector2D> activatedSquare )
        {
            this.puzzle = puzzle;

            var signalFactory = new SignalFactory<Vector2D>( activatedSquare );
            this.squares = Grid.Create( puzzle.Size, p => new GridSquareViewModel( puzzle[p], signalFactory.CreateSignal( p ) ) );
        }

        public IGrid<GridSquareViewModel> Squares
        {
            get
            {
                return squares;
            }
        }
    }
}
