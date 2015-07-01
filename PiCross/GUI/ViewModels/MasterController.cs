using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;
using PiCross.Facade.IO;

namespace GUI.ViewModels
{
    public class MasterController
    {
        private readonly Cell<ViewModel> activeViewModel;

        private readonly IPlayerDatabase playerDatabase;

        public MasterController()
        {
            activeViewModel = Cell.Create<ViewModel>( new IntroViewModel( this ) );

            var dummy = new DummyData();
            playerDatabase = dummy.Players;
        }

        public Cell<ViewModel> ActiveViewModel
        {
            get
            {
                return activeViewModel;
            }
        }

        public IPlayerDatabase PlayerDatabase
        {
            get
            {
                return playerDatabase;
            }
        }
    }
}
