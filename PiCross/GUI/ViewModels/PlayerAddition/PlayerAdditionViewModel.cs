using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;

namespace GUI.ViewModels.PlayerAddition
{
    public class PlayerAdditionViewModel : ViewModel
    {
        private readonly Cell<string> playerName;

        public PlayerAdditionViewModel( MasterController parent )
            : base( parent )
        {
            this.playerName = Cell.Create( "" );
        }

        public Cell<string> PlayerName
        {
            get
            {
                return playerName;
            }
        }
    }
}
