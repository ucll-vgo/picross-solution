using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Solving;
using PiCross.Game;

namespace GUI.ViewModels.PuzzleMode
{
    public class ConstraintsValueViewModel : IPuzzleConstraintsValueViewModel
    {
        private readonly IPuzzleConstraintsValue value;

        public ConstraintsValueViewModel( IPuzzleConstraintsValue value )
        {
            this.value = value;
        }

        public Cell<bool> IsSatisfied
        {
            get
            {
                return this.value.IsSatisfied;
            }
        }

        public Cell<int> Number
        {
            get
            {
                return Cell.Create( this.value.Value );
            }
        }
    }
}
