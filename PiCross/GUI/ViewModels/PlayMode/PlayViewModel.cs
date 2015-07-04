using System.Diagnostics;
using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using GUI.ViewModels.PauseScreen;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Playing;

namespace GUI.ViewModels.PlayMode
{
    public class PlayViewModel : ViewModel, IPuzzleData
    {
        private readonly IPuzzle puzzle;

        private readonly GridViewModel grid;

        private readonly ISequence<ConstraintsViewModel> columnConstraints;

        private readonly ISequence<ConstraintsViewModel> rowConstraints;

        private readonly Cell<Vector2D> activatedSquare;

        public readonly ICommand back;

        private readonly ICommand pause;

        public PlayViewModel( MasterController parent, IPuzzle puzzle )
            : base( parent )
        {
            this.puzzle = puzzle;
            this.activatedSquare = Cell.Create<Vector2D>( null );
            this.grid = new GridViewModel( puzzle, activatedSquare );
            this.columnConstraints = puzzle.ColumnConstraints.Map( ( i, columnConstraints ) => new ConstraintsViewModel( columnConstraints, Cell.Derived( activatedSquare, v => v != null && v.X == i ) ) ).Copy();
            this.rowConstraints = puzzle.RowConstraints.Map( ( i, rowConstraints ) => new ConstraintsViewModel( rowConstraints, Cell.Derived( activatedSquare, v => v != null && v.Y == i ) ) ).Copy();
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.pause = EnabledCommand.FromDelegate( PerformPause );
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

        IGrid<object> IPuzzleData.Grid
        {
            get
            {
                return Grid.Squares;
            }
        }

        ISequence<object> IPuzzleData.ColumnConstraints
        {
            get
            {
                return columnConstraints;
            }
        }

        ISequence<object> IPuzzleData.RowConstraints
        {
            get
            {
                return rowConstraints;
            }
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }

        private void PerformBack()
        {
            Pop();
        }

        public ICommand Pause
        {
            get
            {
                return pause;
            }
        }

        private void PerformPause()
        {
            Push( new PauseViewModel( Parent ) );
        }
    }
}
