using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Solving;
using PiCross.Game;

namespace GUI.ViewModels
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

    public class GridViewModel : IPuzzleGridViewModel
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

    public class GridSquareViewModel : IPuzzleGridSquareViewModel
    {
        private readonly IPuzzleSquare square;

        private readonly ICommand toggle;

        private readonly ICommand toggleEmpty;

        private readonly ICommand toggleFilled;

        private readonly ICommand activate;

        public GridSquareViewModel( IPuzzleSquare square, ISignal activationSignal )
        {
            this.square = square;
            this.toggle = new ToggleCommand( square.Contents );
            this.toggleEmpty = new ToggleEmptyCommand( square.Contents );
            this.toggleFilled = new ToggleFilledCommand( square.Contents );
            this.activate = new ActivationCommand( activationSignal );
        }

        public Cell<Square> Contents
        {
            get
            {
                return square.Contents;
            }
        }

        public ICommand Activate
        {
            get
            {
                return activate;
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

        private class ToggleCommand : EnabledCommand
        {
            private readonly Cell<Square> square;

            public ToggleCommand( Cell<Square> square )
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

        private class ToggleEmptyCommand : EnabledCommand
        {
            private readonly Cell<Square> square;

            public ToggleEmptyCommand( Cell<Square> square )
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

        private class ToggleFilledCommand : EnabledCommand
        {
            private readonly Cell<Square> square;

            public ToggleFilledCommand( Cell<Square> square )
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

    public class ConstraintsViewModel : IPuzzleConstraintsViewModel
    {
        private readonly IPuzzleConstraints constraints;

        private readonly ISequence<ConstraintsValueViewModel> values;

        private readonly Cell<bool> active;

        public ConstraintsViewModel( IPuzzleConstraints constraints, Cell<bool> active )
        {
            this.constraints = constraints;
            this.active = active;
            this.values = constraints.Values.Map( val => new ConstraintsValueViewModel( val ) ).Copy();
        }

        ISequence<IPuzzleConstraintsValueViewModel> IPuzzleConstraintsViewModel.Values
        {
            get
            {
                return values;
            }
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

        public Cell<bool> Active
        {
            get
            {
                return active;
            }
        }
    }

    public class ConstraintsValueViewModel : IPuzzleConstraintsValueViewModel
    {
        private readonly IPuzzleConstraintsValue value;

        public ConstraintsValueViewModel( IPuzzleConstraintsValue value )
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
