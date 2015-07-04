using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Playing;

namespace GUI.ViewModels.PlayMode
{
    public class ConstraintsViewModel
    {
        private readonly IPlayablePuzzleConstraints constraints;

        private readonly ISequence<ConstraintsValueViewModel> values;

        private readonly Cell<bool> active;

        public ConstraintsViewModel( IPlayablePuzzleConstraints constraints, Cell<bool> active )
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

        public Cell<bool> IsActive
        {
            get
            {
                return active;
            }
        }
    }

}
