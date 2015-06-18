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
    public class ConstraintsViewModel
    {
        private readonly IPuzzleConstraints constraints;

        private readonly ISequence<ConstraintsValueViewModel> values;

        private readonly Cell<bool> active;

        public ConstraintsViewModel( IPuzzleConstraints constraints, Cell<bool> active )
        {
            this.constraints = constraints;
            this.active = active;
            this.values = constraints.Values.Map( val => new ConstraintsValueViewModel( val ) ).Copy();
        }

        public Cell<bool> IsSatisfied
        {
            get
            {
                return constraints.IsSatisfied;
            }
        }

        public ISequence<ConstraintsValueViewModel> Values
        {
            get
            {
                return values;
            }
        }

        public Cell<bool> Active
        {
            get
            {
                return active;
            }
        }
    }

}
