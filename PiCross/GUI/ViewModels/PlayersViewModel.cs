using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Facade.IO;

namespace GUI.ViewModels
{
    public class PlayersViewModel
    {
        private readonly IPlayerDatabase players;

        private readonly ObservableCollection<PlayerViewModel> playerViewModels;

        private readonly ICommand addPlayer;

        public PlayersViewModel( IPlayerDatabase players )
        {
            this.players = players;
            this.playerViewModels = new ObservableCollection<PlayerViewModel>( players.PlayerNames.Select( name => new PlayerViewModel( players[name] ) ) );
            this.addPlayer = EnabledCommand.FromDelegate<string>( PerformAddPlayer );
        }

        public ObservableCollection<PlayerViewModel> Users
        {
            get
            {
                return playerViewModels;
            }
        }

        private void PerformAddPlayer(string name)
        {
            var profile = players.CreateNewProfile( name );
            playerViewModels.Add( new PlayerViewModel( profile ) );
        }
    }

    public class PlayerViewModel
    {
        private readonly IPlayerProfile userProfile;

        public PlayerViewModel( IPlayerProfile userProfile )
        {
            this.userProfile = userProfile;
        }

        public string Name
        {
            get
            {
                return userProfile.Name;
            }
        }
    }
}
