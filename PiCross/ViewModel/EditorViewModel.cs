using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cells;
using DataStructures;
using PiCross;
using ViewModel.Commands;

namespace ViewModel
{
    public class EditorViewModel : ViewModel
    {
        private readonly IPuzzleLibraryEntry libraryEntry;

        private readonly IPuzzleEditor puzzleEditor;

        private readonly Cell<Vector2D> activeSquare;

        private readonly ICommand resolve;

        private readonly ICommand cancel;

        private readonly ICommand save;

        private readonly ICommand reset;

        private readonly IGrid<Cell<bool>> thumbnailData;

        private readonly PuzzleViewModel puzzleViewModel;

        public EditorViewModel( MasterController parent, IPuzzleLibraryEntry libraryEntry )
            : base( parent )
        {
            this.libraryEntry = libraryEntry;

            this.puzzleEditor = this.Parent.PicrossFacade.CreatePuzzleEditor( libraryEntry.Puzzle );
            this.puzzleViewModel = new PuzzleViewModel( puzzleEditor, activeSquare );
            this.activeSquare = Cell.Create<Vector2D>( null );
            this.resolve = EnabledCommand.FromDelegate( PerformRefine );
            this.cancel = EnabledCommand.FromDelegate( PerformCancel );
            this.save = EnabledCommand.FromDelegate( PerformSave );
            this.reset = EnabledCommand.FromDelegate( PerformReset );
            this.thumbnailData = DataStructures.Grid.Create( puzzleEditor.Grid.Size, position => puzzleEditor.Grid[position].IsFilled );
        }

        public ICommand Cancel
        {
            get
            {
                return cancel;
            }
        }

        public ICommand Save
        {
            get
            {
                return save;
            }
        }

        public ICommand Resolve
        {
            get
            {
                return resolve;
            }
        }

        public ICommand Reset
        {
            get
            {
                return reset;
            }
        }

        public IGrid<Cell<bool>> ThumbnailData
        {
            get
            {
                return thumbnailData;
            }
        }

        private void PerformCancel()
        {
            Pop();
        }

        private void PerformSave()
        {
            this.libraryEntry.Puzzle = this.puzzleEditor.BuildPuzzle();
            Pop();
        }

        private void PerformRefine()
        {
            this.puzzleEditor.ResolveAmbiguity();
        }

        private void PerformReset()
        {
            this.puzzleEditor.Grid.ForEach( ( IPuzzleEditorSquare square ) => { square.IsFilled.Value = false; } );
        }

        public IEditorViewModel ViewModel
        {
            get
            {
                return puzzleViewModel;
            }
        }

        private class PuzzleViewModel : IEditorViewModel
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
                this.grid = DataStructures.Grid.Create<SquareViewModel>( puzzleEditor.Grid.Size, position => new SquareViewModel( puzzleEditor.Grid[position], signalFactory.CreateSignal( position ) ) );
                this.rowConstraints = puzzleEditor.RowConstraints.Map( ( index, cs ) => new ConstraintsViewModel( cs, signalFactory.Cell.Map( p => p != null && p.Y == index ) ) );
                this.columnConstraints = puzzleEditor.ColumnConstraints.Map( ( index, cs ) => new ConstraintsViewModel( cs, signalFactory.Cell.Map( p => p != null && p.X == index ) ) );
                this.thumbnail = DataStructures.Grid.Create<Cell<bool>>( puzzleEditor.Grid.Size, position => puzzleEditor.Grid[position].IsFilled );
            }

            public IGrid<IEditorSquareViewModel> Grid
            {
                get { return grid; }
            }

            public ISequence<IEditorConstraintsViewModel> ColumnConstraints
            {
                get { return columnConstraints; }
            }

            public ISequence<IEditorConstraintsViewModel> RowConstraints
            {
                get { return rowConstraints; }
            }

            public IGrid<Cell<bool>> ThumbnailData
            {
                get { return thumbnail; }
            }
        }

        private class SquareViewModel : IEditorSquareViewModel
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

        private class ConstraintsViewModel : IEditorConstraintsViewModel
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
                    return constraints.Constraints.Map( x => x.Values.Items );
                }
            }

            public Cell<bool> IsActive
            {
                get { return isActive; }
            }
        }
    }

    public interface IEditorViewModel
    {
        IGrid<IEditorSquareViewModel> Grid { get; }

        ISequence<IEditorConstraintsViewModel> ColumnConstraints { get; }

        ISequence<IEditorConstraintsViewModel> RowConstraints { get; }

        IGrid<Cell<bool>> ThumbnailData { get; }
    }

    public interface IEditorSquareViewModel
    {
        Cell<bool> IsFilled { get; }

        Cell<Ambiguity> Ambiguity { get; }

        ICommand Activate { get; }

        ICommand SetFilled { get; }

        ICommand SetEmpty { get; }
    }

    public interface IEditorConstraintsViewModel
    {
        Cell<IEnumerable<int>> Constraints { get; }

        Cell<bool> IsActive { get; }
    }
}
