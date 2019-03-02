using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using Cells;

namespace GUI.ViewModels
{
    public class PlayerAdditionViewModel : ViewModel
    {
        private readonly Cell<string> playerName;

        private readonly Cell<bool> isValidName;

        private readonly Cell<bool> isNameTaken;

        private readonly Cell<bool> canAddPlayer;

        private readonly ICommand addPlayer;

        private readonly ICommand cancel;

        public PlayerAdditionViewModel( MasterController parent )
            : base( parent )
        {
            this.playerName = Cell.Create( "" );
            this.isValidName = this.playerName.Map( Parent.GameData.PlayerDatabase.IsValidPlayerName );
            this.isNameTaken = this.playerName.Map( Parent.GameData.PlayerDatabase.PlayerNames.Contains );
            this.canAddPlayer = Cell.Derived( isValidName, isNameTaken, ( valid, taken ) => valid && !taken );            

            this.addPlayer = CellCommand.FromDelegate( canAddPlayer, PerformAddPlayer );
            this.cancel = EnabledCommand.FromDelegate( PerformCancel );
        }

        public Cell<string> PlayerName
        {
            get
            {
                return playerName;
            }
        }

        public ICommand AddPlayer { get { return addPlayer; } }

        public ICommand Cancel { get { return cancel; } }

        public Cell<bool> IsValidName { get { return isValidName; } }
                
        public Cell<bool> IsNameTaken { get { return isNameTaken; } }

        public Cell<bool> CanAddPlayer { get { return canAddPlayer; } }

        private void PerformAddPlayer()
        {
            Debug.Assert( CanAddPlayer.Value );

            Parent.GameData.PlayerDatabase.CreateNewProfile( playerName.Value );

            BackToPlayerSelection();
        }

        private void PerformCancel()
        {
            BackToPlayerSelection();
        }

        private void BackToPlayerSelection()
        {
            Pop();
        }
    }
}
