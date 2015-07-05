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

        private readonly ICommand activate;

        public GridSquareViewModel( IPlayablePuzzleSquare square, ISignal activationSignal, Cell<bool> isPuzzleSolved )
        {
            var isEnabled = Cell.Derived( isPuzzleSolved, x => !x );

            this.square = square;
            this.toggle = new ToggleCommand( square.Contents, isEnabled );
            this.toggleEmpty = new ToggleEmptyCommand( square.Contents, isEnabled );
            this.toggleFilled = new ToggleFilledCommand( square.Contents, isEnabled );
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
