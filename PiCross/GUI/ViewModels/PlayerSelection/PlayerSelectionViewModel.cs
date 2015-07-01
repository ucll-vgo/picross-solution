using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Facade.IO;

namespace GUI.ViewModels.PlayerSelection
{
    public class PlayerSelectionViewModel : ViewModel
    {
        private readonly IPlayerDatabase players;

        private readonly ObservableCollection<ItemViewModel> playerViewModels;

        public PlayerSelectionViewModel( MasterController parent )
            : base( parent )
        {
            this.players = Parent.PlayerDatabase;
            this.playerViewModels = CreateItemViewModels( this, parent.PlayerDatabase );
        }

        private static ObservableCollection<ItemViewModel> CreateItemViewModels( PlayerSelectionViewModel parent, IPlayerDatabase players )
        {
            var result = new ObservableCollection<ItemViewModel>( players.PlayerNames.Select( name => new SelectPlayerViewModel( parent, players[name] ) ) );
            result.Add( new AddPlayerViewModel( parent ) );

            return result;
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return playerViewModels;
            }
        }
    }

    
}
