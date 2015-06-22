using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly Cell<Vector2D> activeSquare;

        public EditorViewModel( PuzzleEditor_ManualAmbiguity puzzleEditor )
        {
            this.puzzleEditor = puzzleEditor;
            this.activeSquare = Cell.Create<Vector2D>( null );
            this.columnConstraints = puzzleEditor.ColumnConstraints.Map( ( x, constraints ) => new EditorConstraintsViewModel( constraints, Cell.Derived( activeSquare, p => p != null && p.X == x ) ) );
            this.rowConstraints = puzzleEditor.RowConstraints.Map( ( y, constraints ) => new EditorConstraintsViewModel( constraints, Cell.Derived( activeSquare, p => p != null && p.Y == y ) ) );

            var signalFactory = new SignalFactory<Vector2D>( activeSquare );
            this.squares = PiCross.DataStructures.Grid.Create( puzzleEditor.Size, position => new EditorSquareViewModel( puzzleEditor[position], signalFactory.CreateSignal( position ) ) );
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
