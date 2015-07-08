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

        public GridViewModel( IPlayablePuzzle puzzle, Cell<Vector2D> activatedSquare, IGrid<Cell<int?>> selectionGrid )
        {
            this.puzzle = puzzle;

            this.squares = Grid.Create( puzzle.Size, p => new GridSquareViewModel( puzzle[p], activatedSquare.Map( q => p == q ), puzzle.IsSolved, selectionGrid[p] ) );
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
