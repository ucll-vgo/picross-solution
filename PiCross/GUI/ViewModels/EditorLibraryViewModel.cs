using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.Editing;
using PiCross.Facade.IO;
using PiCross.Game;

namespace GUI.ViewModels
{
    public class EditorLibraryViewModel : ViewModel
    {
        private readonly ILibrary library;

        private readonly ICommand back;

        private readonly ICommand select;

        private readonly Cell<IEnumerable<GroupViewModel>> groups;

        public EditorLibraryViewModel( MasterController parent, ILibrary library ) 
            : base(parent)
        {
            this.library = library;
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.select = EnabledCommand.FromDelegate<EntryViewModel>( PerformSelect );
            this.groups = Cell.Create( from entry in library.Entries
                                       group entry by entry.Puzzle.Size into entryGroup
                                       select new GroupViewModel( entryGroup.Key, entryGroup ) );
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }

        public ICommand Select
        {
            get
            {
                return select;
            }
        }

        private void PerformBack()
        {
            Pop();
        }

        private void PerformSelect(EntryViewModel entry)
        {
            var editorGrid = new EditorGrid( entry.Grid.Map( b => b.Value ? Square.FILLED : Square.EMPTY ) );
            var puzzleEditor = new PuzzleEditor_ManualAmbiguity( editorGrid );
            Push( new EditorViewModel( Parent, puzzleEditor ) );
        }

        public Cell<IEnumerable<GroupViewModel>> Groups
        {
            get
            {
                return groups;
            }
        }
        
        public class GroupViewModel
        {
            private readonly Size size;

            private readonly IEnumerable<ILibraryEntry> entries;

            public GroupViewModel(Size size, IEnumerable<ILibraryEntry> entries)
            {
                this.size = size;
                this.entries = entries;
            }

            public Size Size
            {
                get
                {
                    return size;
                }
            }

            public IEnumerable<EntryViewModel> Members
            {
                get
                {
                    return from entry in entries
                           select new EntryViewModel( entry );
                }
            }
        }

        public class EntryViewModel
        {
            private readonly ILibraryEntry entry;

            public EntryViewModel(ILibraryEntry entry)
            {
                this.entry = entry;
            }

            public IGrid<Cell<bool>> Grid
            {
                get
                {
                    return entry.Puzzle.Grid.Map((bool x) => Cell.Create(x)).Copy();
                }
            }
        }
    }
}
