using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels.LibraryMode;
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

            var collection = new ObservableCollection<ItemViewModel>();

            foreach ( var playerName in players.PlayerNames )
            {
                var command = EnabledCommand.FromDelegate( () => PerformSelectPlayer( playerName ) );
                var vm = new SelectPlayerViewModel( this, players[playerName], command );

                collection.Add( vm );
            }

            collection.Add( new AddPlayerViewModel( this, addPlayer ) );

            return collection;
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

        private void PerformSelectPlayer( string playerName )
        {
            Debug.Assert( playerName != null );            

            var library = Parent.Library;
            var playerProfile = Parent.PlayerDatabase[playerName];
            var libraryVM = new LibraryViewModel( Parent, library, playerProfile );

            Switch( libraryVM );
        }
    }
}
