using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;

namespace GUI.ViewModels.EditMode
{
    public class EditorConstraintsViewModel
    {
        private readonly IPuzzleEditorConstraints constraints;

        public EditorConstraintsViewModel( IPuzzleEditorConstraints constraints )
        {
            this.constraints = constraints;
        }

        public Cell<ISequence<int>> Values
        {
            get
            {
                return constraints.Values;
            }
        }
    }
}
