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

        private readonly Stack<ViewModel> viewModelStack;

        private readonly IPlayerDatabase playerDatabase;

        private readonly ILibrary library;

        public MasterController()
        {
            this.viewModelStack = new Stack<ViewModel>();
            viewModelStack.Push( new IntroViewModel( this ) );
            activeViewModel = Cell.Derived( () => viewModelStack.Peek() );

            var dummy = new DummyData();
            this.playerDatabase = dummy.Players;
            this.library = dummy.Puzzles;
        }

        public Cell<ViewModel> ActiveViewModel
        {
            get
            {
                return activeViewModel;
            }
        }

        public void PushViewModel(ViewModel vm)
        {
            viewModelStack.Push( vm );

            OnViewModelStackChanged();
        }

        public void PopViewModel()
        {
            viewModelStack.Pop();

            OnViewModelStackChanged();
        }

        private void OnViewModelStackChanged()
        {
            activeViewModel.Refresh();
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
