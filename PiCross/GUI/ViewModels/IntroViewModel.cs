using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Facade.IO;

namespace GUI.ViewModels
{
    public class IntroViewModel : ViewModel
    {
        private readonly ICommand start;

        public IntroViewModel( MasterController parent )
            : base( parent )
        {
            start = EnabledCommand.FromDelegate( PerformStart );
        }

        public ICommand Start
        {
            get
            {
                return start;
            }
        }

        private void PerformStart()
        {
            Switch( new PlayerSelectionViewModel( Parent ) );
        }
    }
}
