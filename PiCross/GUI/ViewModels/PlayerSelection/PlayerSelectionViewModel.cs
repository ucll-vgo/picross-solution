using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels.PlayerAddition;
using PiCross.Facade.IO;

namespace GUI.ViewModels.PlayerSelection
{
    public class PlayerSelectionViewModel : ViewModel
    {
        private readonly IPlayerDatabase players;

        private readonly ObservableCollection<ItemViewModel> playerViewModels;

        private readonly ICommand addPlayer;

        public PlayerSelectionViewModel( MasterController parent )
            : base( parent )
        {
            this.players = Parent.PlayerDatabase;
            this.addPlayer = EnabledCommand.FromDelegate( PerformAddPlayer );
            this.playerViewModels = CreateItemViewModels();
        }

        private ObservableCollection<ItemViewModel> CreateItemViewModels()
        {
            Debug.Assert( players != null );
            Debug.Assert( addPlayer != null );

            var result = new ObservableCollection<ItemViewModel>( players.PlayerNames.Select( name => new SelectPlayerViewModel( this, players[name] ) ) );
            result.Add( new AddPlayerViewModel( this, addPlayer ) );

            return result;
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return playerViewModels;
            }
        }

        private void PerformAddPlayer()
        {
            Switch( new PlayerAdditionViewModel( Parent ) );
        }
    }
}
