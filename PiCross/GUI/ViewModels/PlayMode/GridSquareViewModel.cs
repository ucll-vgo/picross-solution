using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Playing;
using PiCross.Game;

namespace GUI.ViewModels.PlayMode
{
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
}
