using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;

namespace GUI.ViewModels.LibraryMode
{
    class FilterViewModel : ViewModel
    {
        private Cell<bool> flag;

        public FilterViewModel( MasterController masterController, Cell<bool> showSolved )
            : base( masterController )
        {
            flag = showSolved;
        }

        public Cell<bool> Flag
        {
            get
            {
                return flag;
            }
        }
    }
}
