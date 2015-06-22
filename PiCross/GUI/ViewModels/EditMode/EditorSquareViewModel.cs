using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Game;

namespace GUI.ViewModels.EditMode
{
    public class EditorSquareViewModel
    {
        private readonly IPuzzleEditorSquare square;

        private readonly Cell<bool> isFilled;

        private readonly ICommand setFilled;

        private readonly ICommand setEmpty;

        private readonly ICommand activate;

        public EditorSquareViewModel( IPuzzleEditorSquare square, ISignal signal )
        {
            this.square = square;
            this.isFilled = square.IsFilled;
            this.setFilled = EnabledCommand.FromDelegate( () => isFilled.Value = true );
            this.setEmpty = EnabledCommand.FromDelegate( () => isFilled.Value = false );
            this.activate = new ActivateCommand( signal );
        }

        public Cell<bool> IsFilled
        {
            get
            {
                return isFilled;
            }
        }

        public Cell<Ambiguity> Ambiguity
        {
            get
            {
                return square.Ambiguity;
            }
        }

        public ICommand SetFilled { get { return setFilled; } }

        public ICommand SetEmpty { get { return setEmpty; } }

        public ICommand Activate { get { return activate; } }

        public class ActivateCommand : EnabledCommand
        {
            private readonly ISignal signal;

            public ActivateCommand(ISignal signal)
            {
                this.signal = signal;
            }

            public override void Execute( object parameter )
            {
                signal.Send();
            }
        }
    }
}
