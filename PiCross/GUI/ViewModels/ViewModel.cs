using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public abstract class ViewModel
    {
        private readonly MasterController parent;

        protected ViewModel(MasterController parent)
        {
            this.parent = parent;
        }

        protected MasterController Parent
        {
            get
            {
                return parent;
            }
        }
    }
}
