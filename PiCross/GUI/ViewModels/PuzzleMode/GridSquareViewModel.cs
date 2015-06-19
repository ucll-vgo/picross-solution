using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Solving;
using PiCross.Game;

namespace GUI.ViewModels.PuzzleMode
{
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
}
