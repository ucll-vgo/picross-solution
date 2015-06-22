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
using PiCross.Facade.Editing;
using PiCross.Game;

namespace GUI.ViewModels.EditMode
{
    public class EditorViewModel : IPuzzleData
    {
        private readonly PuzzleEditor_ManualAmbiguity puzzleEditor;

        private readonly IGrid<EditorSquareViewModel> squares;

        private readonly ISequence<EditorConstraintsViewModel> columnConstraints;

        private readonly ISequence<EditorConstraintsViewModel> rowConstraints;

        public EditorViewModel( PuzzleEditor_ManualAmbiguity puzzleEditor )
        {
            this.puzzleEditor = puzzleEditor;
            this.squares = PiCross.DataStructures.Grid.Create( puzzleEditor.Size, position => new EditorSquareViewModel( puzzleEditor[position] ) );
            this.columnConstraints = puzzleEditor.ColumnConstraints.Map( constraints => new EditorConstraintsViewModel( constraints ) );
            this.rowConstraints = puzzleEditor.RowConstraints.Map( constraints => new EditorConstraintsViewModel( constraints ) );
        }

        public IGrid<object> Grid
        {
            get { return squares; }
        }

        public ISequence<EditorConstraintsViewModel> ColumnConstraints
        {
            get
            {
                return columnConstraints;
            }
        }

        public ISequence<EditorConstraintsViewModel> RowConstraints
        {
            get
            {
                return rowConstraints;
            }
        }

        ISequence<object> IPuzzleData.ColumnConstraints
        {
            get
            {
                return columnConstraints;
            }
        }

        ISequence<object> IPuzzleData.RowConstraints
        {
            get
            {
                return rowConstraints;
            }
        }
    }
}
