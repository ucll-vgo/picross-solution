using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;
using PiCross;

namespace GUI.ViewModels
{
    public class MasterController
    {
        private readonly Cell<ViewModel> activeViewModel;

        private readonly Stack<ViewModel> viewModelStack;

        private readonly Action quitAction;

        private readonly PiCrossFacade picrossFacade;

        private readonly IGameData gameData;

        public MasterController(Action quitAction)
        {
            this.picrossFacade = new PiCrossFacade();
            this.viewModelStack = new Stack<ViewModel>();
            viewModelStack.Push( new IntroViewModel( this ) );
            activeViewModel = Cell.Derived( () => viewModelStack.Peek() );
            
            gameData = picrossFacade.CreateDummyGameData();
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

        public IGameData GameData
        {
            get
            {
                return gameData;
            }
        }
        
        private void Quit()
        {
            quitAction();
        }

        public PiCrossFacade PicrossFacade
        {
            get
            {
                return picrossFacade;
            }
        }
    }
}
