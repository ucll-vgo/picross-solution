using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.Commands;

namespace ViewModel
{
    public class PauseViewModel : ViewModel
    {
        private readonly ICommand back;

        public PauseViewModel(MasterController parent)
            : base(parent)
        {
            this.back = EnabledCommand.FromDelegate( PerformBack );
        }

        private void PerformBack()
        {
            Pop();
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }
    }
}
