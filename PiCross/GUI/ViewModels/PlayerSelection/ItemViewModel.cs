using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public SelectPlayerViewModel( PlayerSelectionViewModel parent, IPlayerProfile userProfile )
            : base( parent )
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

    public class AddPlayerViewModel : ItemViewModel
    {
        public AddPlayerViewModel( PlayerSelectionViewModel parent )
            : base( parent )
        {
            // NOP
        }
    }
}
