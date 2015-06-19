using System;
using System.Windows.Input;
using PiCross.Cells;

namespace GUI.Commands
{
    public abstract class CellCommand : ICommand
    {
        private readonly Cell<bool> isEnabled;

        protected CellCommand(Cell<bool> isEnabled)
        {
            if ( isEnabled == null )
            {
                throw new ArgumentNullException( "isEnabled" );
            }
            else
            {
                this.isEnabled = isEnabled;

                isEnabled.ValueChanged += () =>
                {
                    if ( CanExecuteChanged != null )
                    {
                        CanExecuteChanged( this, new EventArgs() );
                    }
                };
            }
        }

        public bool CanExecute( object parameter )
        {
            return isEnabled.Value;
        }

        public event EventHandler CanExecuteChanged;

        public abstract void Execute( object parameter );
    }
}
