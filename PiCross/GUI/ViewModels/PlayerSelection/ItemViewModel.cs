using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Facade.IO;

namespace GUI.ViewModels.PlayerSelection
{
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
