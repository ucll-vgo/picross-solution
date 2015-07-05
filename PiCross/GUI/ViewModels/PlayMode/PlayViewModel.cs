using System;
using System.Diagnostics;
using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using GUI.ViewModels.PauseScreen;
using PiCross;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade;
using PiCross.Facade.Playing;

namespace GUI.ViewModels.PlayMode
{
    public class PlayViewModel : ViewModel, IPuzzleData
    {
        private readonly IPlayablePuzzle puzzle;

        private readonly GridViewModel grid;

        private readonly ISequence<ConstraintsViewModel> columnConstraints;

        private readonly ISequence<ConstraintsViewModel> rowConstraints;

        private readonly Cell<Vector2D> activatedSquare;

        public readonly ICommand back;

        private readonly ICommand pause;

        private readonly Chronometer chronometer;

        public PlayViewModel( MasterController parent, IPlayablePuzzle puzzle )
            : base( parent )
        {
            this.puzzle = puzzle;
            this.activatedSquare = Cell.Create<Vector2D>( null );
            this.grid = new GridViewModel( puzzle, activatedSquare );
            this.columnConstraints = puzzle.ColumnConstraints.Map( ( i, columnConstraints ) => new ConstraintsViewModel( columnConstraints, Cell.Derived( activatedSquare, v => v != null && v.X == i ) ) ).Copy();
            this.rowConstraints = puzzle.RowConstraints.Map( ( i, rowConstraints ) => new ConstraintsViewModel( rowConstraints, Cell.Derived( activatedSquare, v => v != null && v.Y == i ) ) ).Copy();
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.pause = EnabledCommand.FromDelegate( PerformPause );
            this.chronometer = new Chronometer();
        }

        public Cell<bool> IsSolved
        {
            get
            {
                return this.puzzle.IsSolved;
            }
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

        public override void OnTick( double dt )
        {
            chronometer.Tick();
        }

        protected override void OnActivation()
        {
            chronometer.Start();
        }

        protected override void OnDeactivation()
        {
            chronometer.Pause();
        }

        public Cell<TimeSpan> ElapsedTime
        {
            get
            {
                return chronometer.TotalTime;
            }
        }
    }
}
