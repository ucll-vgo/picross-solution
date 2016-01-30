using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.Controls;
using Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Game;

namespace GUI.ViewModels
{
    public class EditorViewModel : ViewModel
    {
        private readonly PuzzleEditor_ManualAmbiguity puzzleEditor;

        private readonly Cell<Vector2D> activeSquare;

        private readonly ICommand refine;

        private readonly ICommand back;

        private readonly IGrid<Cell<bool>> thumbnailData;

        private readonly PuzzleViewModel puzzleViewModel;

        public EditorViewModel( MasterController parent, PuzzleEditor_ManualAmbiguity puzzleEditor )
            : base( parent )
        {
            this.puzzleViewModel = new PuzzleViewModel( puzzleEditor, activeSquare );
            
            this.puzzleEditor = puzzleEditor;
            this.activeSquare = Cell.Create<Vector2D>( null );
            this.refine = EnabledCommand.FromDelegate( PerformRefine );
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.thumbnailData = PiCross.DataStructures.Grid.Create( puzzleEditor.Size, position => puzzleEditor[position].IsFilled );
        }

        public ICommand Back
        {
            get
            {
                return back;
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

        private void PerformBack()
        {
            Pop();
        }

        private void PerformRefine()
        {
            this.puzzleEditor.ResolveAmbiguity();
        }

        public EditorControl.IViewModel ViewModel
        {
            get
            {
                return puzzleViewModel;
            }
        }

        private class PuzzleViewModel : EditorControl.IViewModel
        {
            private readonly IPuzzleEditor puzzleEditor;

            private readonly IGrid<SquareViewModel> grid;

            private readonly ISequence<ConstraintsViewModel> rowConstraints;

            private readonly ISequence<ConstraintsViewModel> columnConstraints;

            private readonly IGrid<Cell<bool>> thumbnail;

            public PuzzleViewModel( IPuzzleEditor puzzleEditor, Cell<Vector2D> activeSquare )
            {
                var signalFactory = new SignalFactory<Vector2D>();

                this.puzzleEditor = puzzleEditor;
                this.grid = PiCross.DataStructures.Grid.Create<SquareViewModel>( puzzleEditor.Size, position => new SquareViewModel( puzzleEditor[position], signalFactory.CreateSignal( position ) ) );
                this.rowConstraints = puzzleEditor.RowConstraints.Map( ( index, cs ) => new ConstraintsViewModel( cs, signalFactory.Cell.Map( p => p != null && p.Y == index ) ) );
                this.columnConstraints = puzzleEditor.ColumnConstraints.Map( ( index, cs ) => new ConstraintsViewModel( cs, signalFactory.Cell.Map( p => p != null && p.X == index ) ) );
                this.thumbnail = PiCross.DataStructures.Grid.Create<Cell<bool>>( puzzleEditor.Size, position => puzzleEditor[position].IsFilled );
            }

            public IGrid<EditorControl.ISquareViewModel> Grid
            {
                get { return grid; }
            }

            public ISequence<EditorControl.IConstraintsViewModel> ColumnConstraints
            {
                get { return columnConstraints; }
            }

            public ISequence<EditorControl.IConstraintsViewModel> RowConstraints
            {
                get { return rowConstraints; }
            }
            
            public IGrid<Cell<bool>> ThumbnailData
            {
                get { return thumbnail; }
            }
        }

        private class SquareViewModel : EditorControl.ISquareViewModel
        {
            private readonly IPuzzleEditorSquare square;

            private readonly ISignal activationSignal;

            private readonly ICommand activate;

            private readonly ICommand fill;

            private readonly ICommand empty;

            public SquareViewModel( IPuzzleEditorSquare square, ISignal signal )
            {
                this.square = square;
                this.activationSignal = signal;
                this.activate = EnabledCommand.FromDelegate( () => activationSignal.Send() );
                this.fill = EnabledCommand.FromDelegate( () => { square.IsFilled.Value = true; } );
                this.empty = EnabledCommand.FromDelegate( () => { square.IsFilled.Value = false; } );
            }

            public Cell<bool> IsFilled
            {
                get
                {
                    return square.IsFilled;
                }
            }

            public Cell<Ambiguity> Ambiguity
            {
                get { return square.Ambiguity; }
            }

            public ICommand Activate
            {
                get { return activate; }
            }

            public ICommand SetFilled
            {
                get { return fill; }
            }

            public ICommand SetEmpty
            {
                get { return empty; }
            }
        }

        private class ConstraintsViewModel : EditorControl.IConstraintsViewModel
        {
            private readonly IPuzzleEditorConstraints constraints;

            private readonly Cell<bool> isActive;

            public ConstraintsViewModel( IPuzzleEditorConstraints constraints, Cell<bool> isActive )
            {
                this.constraints = constraints;
                this.isActive = isActive;
            }

            public Cell<IEnumerable<int>> Constraints
            {
                get
                {
                    return constraints.Constraints.Map(x => x.Values.Items);
                }
            }

            public Cell<bool> IsActive
            {
                get { return isActive; }
            }
        }
    }
}
