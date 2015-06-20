using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Controls;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Game;

namespace GUI.ViewModels.EditMode
{
    public class EditorViewModel : IPuzzleData
    {
        private readonly IPuzzleEditor puzzleEditor;

        private readonly IGrid<EditorSquareViewModel> squares;

        private readonly ISequence<EditorConstraintsViewModel> columnConstraints;

        private readonly ISequence<EditorConstraintsViewModel> rowConstraints;

        public EditorViewModel( IPuzzleEditor puzzleEditor )
        {
            this.puzzleEditor = puzzleEditor;
            this.squares = PiCross.DataStructures.Grid.Create( puzzleEditor.Width, puzzleEditor.Height, position => new EditorSquareViewModel( puzzleEditor[position] ) );
            this.columnConstraints = puzzleEditor.ColumnConstraints.Map( constraints => new EditorConstraintsViewModel( constraints ) );
            this.rowConstraints = puzzleEditor.RowConstraints.Map( constraints => new EditorConstraintsViewModel( constraints ) );
        }

        public IGrid<object> Grid
        {
            get { return squares; }
        }

        public ISequence<object> ColumnConstraints
        {
            get
            {
                return columnConstraints;
            }
        }

        public ISequence<object> RowConstraints
        {
            get
            {
                return rowConstraints;
            }
        }
    }

    public class EditorSquareViewModel
    {
        private readonly IPuzzleEditorSquare square;

        private readonly Cell<bool> isFilled;

        public EditorSquareViewModel( IPuzzleEditorSquare square )
        {
            this.square = square;
            this.isFilled = Cell.Derived( square.Contents, x => x == Square.FILLED );
        }

        public Cell<bool> IsFilled
        {
            get
            {
                return isFilled;
            }
        }
    }

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
