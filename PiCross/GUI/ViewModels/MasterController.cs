using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;
using PiCross.Facade;

namespace GUI.ViewModels
{
    public class MasterController
    {
        private readonly Cell<ViewModel> activeViewModel;

        private readonly Stack<ViewModel> viewModelStack;

        private readonly IPlayerDatabase playerDatabase;

        private readonly IPuzzleLibrary library;

        private readonly Action quitAction;

        public MasterController(Action quitAction)
        {
            this.viewModelStack = new Stack<ViewModel>();
            viewModelStack.Push( new IntroViewModel( this ) );
            activeViewModel = Cell.Derived( () => viewModelStack.Peek() );

            var facade = new PiCrossFacade();
            var data = facade.CreateDummyGameData();
            this.library = data.PuzzleLibrary;
            this.playerDatabase = data.PlayerDatabase;
            this.quitAction = quitAction;
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

            if ( viewModelStack.Count == 0 )
            {
                Quit();
            }
            else
            {
                OnViewModelStackChanged();
            }
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

        public IPuzzleLibrary Library
        {
            get
            {
                return library;
            }
        }
        
        private void Quit()
        {
            quitAction();
        }
    }
}
