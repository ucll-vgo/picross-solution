using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels.LibraryMode;
using GUI.ViewModels.PlayerSelection;
using PiCross.Facade.IO;

namespace GUI.ViewModels.Intro
{
    public class IntroViewModel : ViewModel
    {
        private readonly ICommand play;

        private readonly ICommand edit;

        private readonly ICommand quit;

        public IntroViewModel( MasterController parent )
            : base( parent )
        {
            play = EnabledCommand.FromDelegate( PerformPlay );
            edit = EnabledCommand.FromDelegate( PerformEdit );
            quit = EnabledCommand.FromDelegate( PerformQuit );
        }

        public ICommand Play
        {
            get
            {
                return play;
            }
        }

        public ICommand Edit
        {
            get
            {
                return edit;
            }
        }

        public ICommand Quit
        {
            get
            {
                return quit;
            }
        }

        private void PerformPlay()
        {
            Push( new PlayerSelectionViewModel( Parent ) );
        }

        private void PerformEdit()
        {
            Push( new EditorLibraryViewModel( Parent, Parent.Library ) );
        }

        private void PerformQuit()
        {
            Pop();
        }
    }
}
