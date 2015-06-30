using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Facade.IO;

namespace GUI.ViewModels
{
    public class PlayersViewModel
    {
        private readonly IPlayerDatabase players;

        private readonly List<PlayerViewModel> playerViewModels;

        public PlayersViewModel( IPlayerDatabase players )
        {
            this.players = players;
            this.playerViewModels = players.PlayerNames.Select( name => new PlayerViewModel( players[name] ) ).ToList();
        }

        public IEnumerable<PlayerViewModel> Users
        {
            get
            {
                return playerViewModels;
            }
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
