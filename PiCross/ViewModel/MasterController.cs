using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;
using PiCross;

namespace ViewModel
{
    public class MasterController
    {
        private readonly Cell<ViewModel> activeViewModel;

        private readonly Stack<ViewModel> viewModelStack;

        private readonly PiCrossFacade picrossFacade;

        private readonly IGameData gameData;

        public MasterController()
        {
            this.picrossFacade = new PiCrossFacade();
            this.viewModelStack = new Stack<ViewModel>();
            viewModelStack.Push( new IntroViewModel( this ) );
            activeViewModel = Cell.Derived( () => viewModelStack.Peek() );

            try
            {
                gameData = picrossFacade.LoadGameData( @"e:\test.zip", true );
            }
            catch ( Exception e )
            {
                Debug.WriteLine( "----> FAILED TO LOAD DATA! <----" );
                Debug.WriteLine(e.Message);

                gameData = picrossFacade.CreateDummyGameData();
            }
        }

        public Action QuitAction { get; set; }

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
            QuitAction();
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
