using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Facade.IO;

namespace GUI.ViewModels.LibraryMode
{
    public class EditorLibraryViewModel : ViewModel
    {
        private readonly ILibrary library;

        private readonly ICommand back;

        public EditorLibraryViewModel( MasterController parent, ILibrary library ) 
            : base(parent)
        {
            this.library = library;
            this.back = EnabledCommand.FromDelegate( PerformBack );
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }

        private void PerformBack()
        {
            Pop();
        }
    }
}
