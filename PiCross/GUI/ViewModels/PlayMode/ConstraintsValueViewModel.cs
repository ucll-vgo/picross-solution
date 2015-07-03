using PiCross.Cells;
using PiCross.Facade.Playing;

namespace GUI.ViewModels.PlayMode
{
    public class ConstraintsValueViewModel
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
