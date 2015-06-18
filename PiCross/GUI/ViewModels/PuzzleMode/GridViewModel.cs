using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Commands;
using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Solving;
using PiCross.Game;

namespace GUI.ViewModels.PuzzleMode
{
    public class GridViewModel
    {
        private readonly IPuzzle puzzle;

        private readonly IGrid<GridSquareViewModel> squares;

        public GridViewModel( IPuzzle puzzle, Cell<Vector2D> activatedSquare )
        {
            this.puzzle = puzzle;

            var signalFactory = new SignalFactory<Vector2D>( activatedSquare );
            this.squares = Grid.Create( puzzle.Width, puzzle.Height, p => new GridSquareViewModel( puzzle[p], signalFactory.CreateSignal( p ) ) );
        }

        public IGrid<IPuzzleGridSquareViewModel> Squares
        {
            get
            {
                return squares;
            }
        }
    }
}
