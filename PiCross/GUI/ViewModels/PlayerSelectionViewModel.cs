using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross;

namespace GUI.ViewModels
{
    public class PlayerSelectionViewModel : ViewModel
    {
        private readonly IPlayerDatabase players;

        private readonly ObservableCollection<ItemViewModel> playerViewModels;

        private readonly ICommand addPlayer;

        private readonly ICommand back;

        public PlayerSelectionViewModel( MasterController parent )
            : base( parent )
        {
            this.players = Parent.GameData.PlayerDatabase;
            this.addPlayer = EnabledCommand.FromDelegate( PerformAddPlayer );
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.playerViewModels = new ObservableCollection<ItemViewModel>();
        }

        private void RecreateItemViewModels()
        {
            playerViewModels.Clear();

            foreach ( var playerName in players.PlayerNames )
            {
                var command = EnabledCommand.FromDelegate( () => PerformSelectPlayer( playerName ) );
                var vm = new SelectPlayerViewModel( this, players[playerName], command );

                playerViewModels.Add( vm );
            }

            playerViewModels.Add( new AddPlayerViewModel( this, addPlayer ) );
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
            Push( new PlayerAdditionViewModel( Parent ) );
        }

        private void PerformSelectPlayer( string playerName )
        {
            Debug.Assert( playerName != null );

            var library = Parent.GameData.PuzzleLibrary;
            var playerProfile = Parent.GameData.PlayerDatabase[playerName];
            var libraryVM = new PlayLibraryViewModel( Parent, library, playerProfile );

            Push( libraryVM );
        }

        protected override void OnActivation()
        {
            RecreateItemViewModels();
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }
        private void PerformBack()
        {
            Pop();
        }

        public abstract class ItemViewModel
        {
            private readonly PlayerSelectionViewModel parent;

            protected ItemViewModel( PlayerSelectionViewModel parent )
            {
                this.parent = parent;
            }

            protected PlayerSelectionViewModel Parent
            {
                get
                {
                    return parent;
                }
            }
        }

        public class SelectPlayerViewModel : ItemViewModel
        {
            private readonly IPlayerProfile userProfile;

            private readonly ICommand select;

            public SelectPlayerViewModel( PlayerSelectionViewModel parent, IPlayerProfile userProfile, ICommand select )
                : base( parent )
            {
                this.userProfile = userProfile;
                this.select = select;
            }

            public string Name
            {
                get
                {
                    return userProfile.Name;
                }
            }

            public ICommand Select
            {
                get
                {
                    return select;
                }
            }
        }

        public class AddPlayerViewModel : ItemViewModel
        {
            private readonly ICommand addPlayer;

            public AddPlayerViewModel( PlayerSelectionViewModel parent, ICommand addPlayer )
                : base( parent )
            {
                this.addPlayer = addPlayer;
            }

            public ICommand AddPlayer
            {
                get { return addPlayer; }
            }
        }
    }
}
