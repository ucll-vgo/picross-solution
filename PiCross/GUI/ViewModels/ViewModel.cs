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

        protected void Push(ViewModel viewModel)
        {
            Parent.ActiveViewModel.Value.OnDeactivation();
            Parent.PushViewModel( viewModel );
            Parent.ActiveViewModel.Value.OnActivation();
        }

        protected void Pop()
        {
            Parent.ActiveViewModel.Value.OnDeactivation();
            Parent.PopViewModel();
            Parent.ActiveViewModel.Value.OnActivation();
        }

        protected virtual void OnActivation()
        {
            // NOP
        }

        protected virtual void OnDeactivation()
        {
            // NOP
        }
    }
}
