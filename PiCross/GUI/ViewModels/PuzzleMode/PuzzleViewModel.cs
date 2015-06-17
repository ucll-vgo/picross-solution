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
    public class PuzzleViewModel : IPuzzleViewModel
    {
        private readonly IPuzzle puzzle;

        private readonly GridViewModel grid;

        private readonly ISequence<ConstraintsViewModel> columnConstraints;

        private readonly ISequence<ConstraintsViewModel> rowConstraints;

        private readonly Cell<Vector2D> activatedSquare;

        public PuzzleViewModel( IPuzzle puzzle )
        {
            this.puzzle = puzzle;
            this.activatedSquare = Cell.Create<Vector2D>( null );
            this.grid = new GridViewModel( puzzle, activatedSquare );
            this.columnConstraints = puzzle.ColumnConstraints.Map( ( i, columnConstraints ) => new ConstraintsViewModel( columnConstraints, Cell.Derived( activatedSquare, v => v != null && v.X == i ) ) ).Copy();
            this.rowConstraints = puzzle.RowConstraints.Map( ( i, rowConstraints ) => new ConstraintsViewModel( rowConstraints, Cell.Derived( activatedSquare, v => v != null && v.Y == i ) ) ).Copy();
        }

        public IPuzzleGridViewModel Grid
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

        ISequence<IPuzzleConstraintsViewModel> IPuzzleViewModel.ColumnConstraints
        {
            get
            {
                return columnConstraints;
            }
        }

        ISequence<IPuzzleConstraintsViewModel> IPuzzleViewModel.RowConstraints
        {
            get
            {
                return rowConstraints;
            }
        }
    }
}
