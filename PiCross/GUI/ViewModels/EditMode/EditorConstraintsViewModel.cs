using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Game;

namespace GUI.ViewModels.EditMode
{
    public class EditorConstraintsViewModel
    {
        private readonly IPuzzleEditorConstraints constraints;

        private readonly Cell<bool> isActive;

        public EditorConstraintsViewModel( IPuzzleEditorConstraints constraints, Cell<bool> isActive )
        {
            this.constraints = constraints;
            this.isActive = isActive;
        }

        public Cell<Constraints> Values
        {
            get
            {
                return constraints.Values;
            }
        }

        public Cell<bool> IsActive
        {
            get
            {
                return isActive;
            }
        }
    }
}
