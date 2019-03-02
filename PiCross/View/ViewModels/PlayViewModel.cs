using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using PiCross;
using Cells;
using DataStructures;
using Utility;

namespace GUI.ViewModels
{
    public class PlayViewModel : ViewModel, IPuzzleData
    {
        private readonly IPlayablePuzzle puzzle;

        private readonly GridViewModel grid;

        private readonly ISequence<ConstraintsViewModel> columnConstraints;

        private readonly ISequence<ConstraintsViewModel> rowConstraints;

        private readonly Cell<Vector2D> activatedSquare;

        private readonly Chronometer chronometer;

        private readonly IPlayerPuzzleInformation entry;

        private readonly GridSelectionHelper selectionHelper;

        public PlayViewModel( MasterController parent, IPlayablePuzzle puzzle, IPlayerPuzzleInformation entry )
            : base( parent )
        {
            this.puzzle = puzzle;
            this.activatedSquare = Cell.Create<Vector2D>( null );
            this.selectionHelper = new GridSelectionHelper( puzzle.Grid.Size );
            this.grid = new GridViewModel( puzzle, activatedSquare, selectionHelper.Selection );
            this.columnConstraints = CreateColumnConstraints();
            this.rowConstraints = CreateRowConstraints();
            this.chronometer = new Chronometer();
            this.entry = entry;

            // Commands
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.pause = CellCommand.FromDelegate( this.puzzle.IsSolved.Negate(), PerformPause );

            SubscribeListeners();
        }

        private ISequence<ConstraintsViewModel> CreateColumnConstraints()
        {
            Debug.Assert( puzzle != null );
            Debug.Assert( activatedSquare != null );

            return puzzle.ColumnConstraints.Map( ( i, columnConstraints ) => new ConstraintsViewModel( columnConstraints, activatedSquare.Map( v => v != null && v.X == i ) ) ).Copy();
        }

        private ISequence<ConstraintsViewModel> CreateRowConstraints()
        {
            Debug.Assert( puzzle != null );
            Debug.Assert( activatedSquare != null );

            return puzzle.RowConstraints.Map( ( i, rowConstraints ) => new ConstraintsViewModel( rowConstraints, activatedSquare.Map( v => v != null && v.Y == i ) ) ).Copy();
        }

        private void SubscribeListeners()
        {
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
            if ( !entry.BestTime.HasValue || chronometer.TotalTime.Value < entry.BestTime.Value )
            {
                entry.BestTime = chronometer.TotalTime.Value;
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

        public TimeSpan? BestTime
        {
            get
            {
                return entry.BestTime;
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

        public Cell<Vector2D> ActivatedSquare
        {
            get
            {
                return activatedSquare;
            }
        }

        public Cell<int> UnknownCount
        {
            get
            {
                return this.puzzle.UnknownCount;
            }
        }

        public Cell<bool> ContainsUnknowns
        {
            get
            {
                return this.puzzle.ContainsUnknowns;
            }
        }

        #region Multiselect Logic

        private Square newContents;

        public void StartSelection( Vector2D position, bool fillMode )
        {
            if ( !IsSolved.Value )
            {
                var oldContents = Grid.Squares[position].Contents.Value;

                if ( fillMode )
                {
                    newContents = oldContents != Square.FILLED ? Square.FILLED : Square.UNKNOWN;
                }
                else
                {
                    newContents = oldContents != Square.EMPTY ? Square.EMPTY : Square.UNKNOWN;
                }

                selectionHelper.SelectionStart = position;
                selectionHelper.SelectionEnd = position;
            }
        }

        public void DragSelection( Vector2D position )
        {
            if ( !IsSolved.Value )
            {
                selectionHelper.SelectionEnd = position;
            }
        }

        public void EndSelection()
        {
            if ( !IsSolved.Value )
            {
                foreach ( var position in Grid.Squares.AllPositions )
                {
                    Grid.Squares[position].SetContentsIfSelected( newContents );
                }

                selectionHelper.SelectionStart = null;
                selectionHelper.SelectionEnd = null;
            }
        }

        #endregion

        public class GridViewModel
        {
            private readonly IPlayablePuzzle puzzle;

            private readonly IGrid<GridSquareViewModel> squares;

            public GridViewModel( IPlayablePuzzle puzzle, Cell<Vector2D> activatedSquare, IGrid<Cell<int?>> selectionGrid )
            {
                this.puzzle = puzzle;

                this.squares = DataStructures.Grid.Create( puzzle.Grid.Size, p => new GridSquareViewModel( puzzle.Grid[p], activatedSquare.Map( q => p == q ), puzzle.IsSolved, selectionGrid[p] ) );
            }

            public IGrid<GridSquareViewModel> Squares
            {
                get
                {
                    return squares;
                }
            }
        }

        public class GridSquareViewModel
        {
            private readonly IPlayablePuzzleSquare square;

            private readonly ICommand toggle;

            private readonly ICommand toggleEmpty;

            private readonly ICommand toggleFilled;

            private readonly Cell<bool> isActive;

            private readonly Cell<int?> selectionIndex;

            private readonly Cell<bool> isSelected;

            public GridSquareViewModel( IPlayablePuzzleSquare square, Cell<bool> isActive, Cell<bool> isPuzzleSolved, Cell<int?> selectionIndex )
            {
                var isEnabled = isPuzzleSolved.Negate();

                this.square = square;
                this.toggle = new ToggleCommand( square.Contents, isEnabled );
                this.toggleEmpty = new ToggleEmptyCommand( square.Contents, isEnabled );
                this.toggleFilled = new ToggleFilledCommand( square.Contents, isEnabled );
                this.isActive = isActive;
                this.selectionIndex = selectionIndex.Map( x => x.HasValue ? (int?) ( x.Value + 1 ) : null );
                this.isSelected = this.selectionIndex.Map( x => x.HasValue );
            }

            public Vector2D Position
            {
                get
                {
                    return square.Position;
                }
            }

            public Cell<int?> SelectionIndex
            {
                get
                {
                    return selectionIndex;
                }
            }

            public Cell<bool> IsSelected
            {
                get
                {
                    return isSelected;
                }
            }

            public Cell<Square> Contents
            {
                get
                {
                    return square.Contents;
                }
            }

            public Cell<bool> IsActive
            {
                get
                {
                    return isActive;
                }
            }

            public ICommand Toggle
            {
                get
                {
                    return toggle;
                }
            }

            public ICommand ToggleEmpty
            {
                get
                {
                    return toggleEmpty;
                }
            }

            public ICommand ToggleFilled
            {
                get
                {
                    return toggleFilled;
                }
            }

            public void SetContentsIfSelected( Square square )
            {
                if ( selectionIndex.Value.HasValue )
                {
                    this.square.Contents.Value = square;
                }
            }

            private class ToggleCommand : CellCommand
            {
                private readonly Cell<Square> square;

                public ToggleCommand( Cell<Square> square, Cell<bool> isEnabled )
                    : base( isEnabled )
                {
                    this.square = square;
                }

                public override void Execute( object parameter )
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

            private class ToggleEmptyCommand : CellCommand
            {
                private readonly Cell<Square> square;

                public ToggleEmptyCommand( Cell<Square> square, Cell<bool> isEnabled )
                    : base( isEnabled )
                {
                    this.square = square;
                }

                public override void Execute( object parameter )
                {
                    var contents = square.Value;
                    Square newContents;

                    if ( contents != Square.EMPTY )
                    {
                        newContents = Square.EMPTY;
                    }
                    else
                    {
                        newContents = Square.UNKNOWN;
                    }

                    square.Value = newContents;
                }
            }

            private class ToggleFilledCommand : CellCommand
            {
                private readonly Cell<Square> square;

                public ToggleFilledCommand( Cell<Square> square, Cell<bool> isEnabled )
                    : base( isEnabled )
                {
                    this.square = square;
                }

                public override void Execute( object parameter )
                {
                    var contents = square.Value;
                    Square newContents;

                    if ( contents != Square.FILLED )
                    {
                        newContents = Square.FILLED;
                    }
                    else
                    {
                        newContents = Square.UNKNOWN;
                    }

                    square.Value = newContents;
                }
            }

            private class ActivationCommand : EnabledCommand
            {
                private readonly ISignal activationSignal;

                public ActivationCommand( ISignal activationSignal )
                {
                    this.activationSignal = activationSignal;
                }

                public override void Execute( object parameter )
                {
                    activationSignal.Send();
                }
            }
        }

        public class ConstraintsViewModel
        {
            private readonly IPlayablePuzzleConstraints constraints;

            private readonly ISequence<ConstraintsValueViewModel> values;

            private readonly Cell<bool> active;

            public ConstraintsViewModel( IPlayablePuzzleConstraints constraints, Cell<bool> active )
            {
                this.constraints = constraints;
                this.active = active;
                this.values = constraints.Values.Map( val => new ConstraintsValueViewModel( val ) ).Copy();
            }

            public Cell<bool> IsSatisfied
            {
                get
                {
                    return constraints.IsSatisfied;
                }
            }

            public ISequence<ConstraintsValueViewModel> Values
            {
                get
                {
                    return values;
                }
            }

            public Cell<bool> IsActive
            {
                get
                {
                    return active;
                }
            }
        }

        public class ConstraintsValueViewModel
        {
            private readonly IPlayablePuzzleConstraintsValue value;

            public ConstraintsValueViewModel( IPlayablePuzzleConstraintsValue value )
            {
                this.value = value;
            }

            public Cell<bool> IsSatisfied
            {
                get
                {
                    return this.value.IsSatisfied;
                }
            }

            public Cell<int> Number
            {
                get
                {
                    return Cell.Create( this.value.Value );
                }
            }
        }
    }
}
