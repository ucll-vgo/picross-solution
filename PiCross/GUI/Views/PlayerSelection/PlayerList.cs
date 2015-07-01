using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels;

namespace GUI.Views.PlayerSelection
{
    public class PlayerList
    {
        private readonly PlayersViewModel players;

        private readonly List<PlayerListItem> playerItems;

        public PlayerList(PlayersViewModel players)
        {
            this.players = players;
            this.playerItems = ( from player in players.Users
                                 select (PlayerListItem) new PlayerSelectionItem( player ) ).ToList();
            this.playerItems.Add( new PlayerAdditionItem(players) );
        }

        public IEnumerable<PlayerListItem> Players
        {
            get
            {
                return playerItems;
            }
        }
    }

    public abstract class PlayerListItem
    {

    }

    public class PlayerSelectionItem : PlayerListItem
    {
        private readonly PlayerViewModel player;

        public PlayerSelectionItem( PlayerViewModel player )
        {
            this.player = player;
        }

        public string Name
        {
            get
            {
                return player.Name;
            }
        }
    }

    public class PlayerAdditionItem : PlayerListItem
    {
        private readonly PlayersViewModel players;

        private readonly ICommand addPlayer;

        public PlayerAdditionItem(PlayersViewModel players)
        {
            this.players = players;
            this.addPlayer = EnabledCommand.FromDelegate( PerformAddPlayer );
        }



        private void PerformAddPlayer()
        {
            // players.a
        }
    }
}
