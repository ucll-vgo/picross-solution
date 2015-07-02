using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.ViewModels.Intro;
using PiCross.Cells;
using PiCross.Facade.IO;

namespace GUI.ViewModels
{
    public class MasterController
    {
        private readonly Cell<ViewModel> activeViewModel;

        private readonly IPlayerDatabase playerDatabase;

        private readonly ILibrary library;

        public MasterController()
        {
            activeViewModel = Cell.Create<ViewModel>( new IntroViewModel( this ) );

            var dummy = new DummyData();
            playerDatabase = dummy.Players;
            library = dummy.Puzzles;
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

        public ILibrary Library
        {
            get
            {
                return library;
            }
        }
    }
}
