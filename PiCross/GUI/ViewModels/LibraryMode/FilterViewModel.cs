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

        public FilterViewModel( MasterController masterController )
            : base( masterController )
        {
            flag = Cell.Create( false );
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
