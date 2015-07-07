using System;
using System.ComponentModel;
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

        private readonly Chronometer chronometer;

        private readonly Cell<TimeSpan?> bestTime;

        public PlayViewModel( MasterController parent, IPlayablePuzzle puzzle, Cell<TimeSpan?> bestTime )
            : base( parent )
        {
            this.puzzle = puzzle;
            this.activatedSquare = Cell.Create<Vector2D>( null );
            this.grid = new GridViewModel( puzzle, activatedSquare );
            this.columnConstraints = puzzle.ColumnConstraints.Map( ( i, columnConstraints ) => new ConstraintsViewModel( columnConstraints, Cell.Derived( activatedSquare, v => v != null && v.X == i ) ) ).Copy();
            this.rowConstraints = puzzle.RowConstraints.Map( ( i, rowConstraints ) => new ConstraintsViewModel( rowConstraints, Cell.Derived( activatedSquare, v => v != null && v.Y == i ) ) ).Copy();
            this.chronometer = new Chronometer();
            this.bestTime = bestTime;

            // Commands
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.pause = EnabledCommand.FromDelegate( PerformPause );

            SubscribeListeners();
        }

        private void SubscribeListeners()
        {
            // ( (INotifyPropertyChanged) this.puzzle.IsSolved ).PropertyChanged += ( sender, args ) => OnIsSolved();
            this.puzzle.IsSolved.ValueChanged += OnIsSolved;
        }

        public Cell<bool> IsSolved
        {
            get
            {
                return this.puzzle.IsSolved;
            }
        }

        private void OnIsSolved()
        {
            SaveTimeIfBetterThanBest();
        }

        private void SaveTimeIfBetterThanBest()
        {
            if ( !bestTime.Value.HasValue || chronometer.TotalTime.Value < bestTime.Value )
            {
                bestTime.Value = chronometer.TotalTime.Value;
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

        public Cell<TimeSpan?> BestTime
        {
            get
            {
                return bestTime;
            }
        }

        #region IPuzzleData explicit implementation

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

        #endregion

        #region Back Command

        private readonly ICommand back;

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

        #endregion

        #region Pause Command

        private readonly ICommand pause;

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

        #endregion

        public override void OnTick( double dt )
        {
            if ( !IsSolved.Value )
            {
                chronometer.Tick();
            }
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
