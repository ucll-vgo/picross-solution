using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels.PlayerSelection;
using PiCross.Facade.IO;

namespace GUI.ViewModels.Intro
{
    public class IntroViewModel : ViewModel
    {
        private readonly ICommand play;

        private readonly ICommand edit;

        public IntroViewModel( MasterController parent )
            : base( parent )
        {
            play = EnabledCommand.FromDelegate( PerformPlay );
            edit = EnabledCommand.FromDelegate( PerformEdit );
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

        private void PerformPlay()
        {
            Push( new PlayerSelectionViewModel( Parent ) );
        }

        private void PerformEdit()
        {
            Push( new PlayerSelectionViewModel( Parent ) );
        }
    }
}
