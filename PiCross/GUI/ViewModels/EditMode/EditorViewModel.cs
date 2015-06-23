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

        private readonly ICommand refine;

        private readonly IGrid<Cell<bool>> thumbnailData;

        public EditorViewModel( PuzzleEditor_ManualAmbiguity puzzleEditor )
        {
            this.puzzleEditor = puzzleEditor;
            this.activeSquare = Cell.Create<Vector2D>( null );
            this.columnConstraints = CreateColumnConstraints( puzzleEditor, activeSquare );
            this.rowConstraints = CreateRowConstraints( puzzleEditor, activeSquare );
            this.squares = CreateSquares( puzzleEditor, activeSquare );
            this.refine = new RefineCommand( this );
            this.thumbnailData = PiCross.DataStructures.Grid.Create( puzzleEditor.Size, position => puzzleEditor[position].IsFilled );
        }

        private static ISequence<EditorConstraintsViewModel> CreateColumnConstraints( IPuzzleEditor puzzleEditor, Cell<Vector2D> activeSquare )
        {
            return puzzleEditor.ColumnConstraints.Map( ( x, constraints ) => new EditorConstraintsViewModel( constraints, Cell.Derived( activeSquare, p => p != null && p.X == x ) ) );
        }

        private static ISequence<EditorConstraintsViewModel> CreateRowConstraints( IPuzzleEditor puzzleEditor, Cell<Vector2D> activeSquare )
        {
            return puzzleEditor.RowConstraints.Map( ( y, constraints ) => new EditorConstraintsViewModel( constraints, Cell.Derived( activeSquare, p => p != null && p.Y == y ) ) );
        }

        private static IGrid<EditorSquareViewModel> CreateSquares( IPuzzleEditor puzzleEditor, Cell<Vector2D> activeSquare )
        {
            var signalFactory = new SignalFactory<Vector2D>( activeSquare );
            return PiCross.DataStructures.Grid.Create( puzzleEditor.Size, position => new EditorSquareViewModel( puzzleEditor[position], signalFactory.CreateSignal( position ) ) );
        }

        IGrid<object> IPuzzleData.Grid
        {
            get { return squares; }
        }

        public IGrid<EditorSquareViewModel> Grid
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

        public ICommand Refine
        {
            get
            {
                return refine;
            }
        }

        public IGrid<Cell<bool>> ThumbnailData
        {
            get
            {
                return thumbnailData;
            }
        }

        private void PerformRefine()
        {
            this.puzzleEditor.ResolveAmbiguity();
        }

        private class RefineCommand : EnabledCommand
        {
            private readonly EditorViewModel parent;

            public RefineCommand( EditorViewModel parent )
            {
                this.parent = parent;
            }

            public override void Execute( object parameter )
            {
                parent.PerformRefine();
            }
        }
    }
}
