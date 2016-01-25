using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Cells;

namespace GUI.ViewModels.LibraryMode
{
    class FilterViewModel : ViewModel
    {
        private Cell<bool> flag;

        private ICommand back;

        public FilterViewModel( MasterController masterController, Cell<bool> showSolved )
            : base( masterController )
        {
            this.flag = showSolved;
            this.back = EnabledCommand.FromDelegate( PerformBack );
        }

        public Cell<bool> Flag
        {
            get
            {
                return flag;
            }
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
