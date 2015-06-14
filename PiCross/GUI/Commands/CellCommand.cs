using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PiCross.DataStructures;

namespace GUI.Commands
{
    public abstract class CellCommand : ICommand
    {
        private readonly ICell<bool> isEnabled;

        protected CellCommand(ICell<bool> isEnabled)
        {
            if ( isEnabled == null )
            {
                throw new ArgumentNullException( "isEnabled" );
            }
            else
            {
                this.isEnabled = isEnabled;

                isEnabled.PropertyChanged += ( sender, args ) =>
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
