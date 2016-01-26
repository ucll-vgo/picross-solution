using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.IO;

namespace GUI.ViewModels.LibraryMode
{
    public class EditorLibraryViewModel : ViewModel
    {
        private readonly ILibrary library;

        private readonly ICommand back;

        private readonly Cell<IEnumerable<Group>> groups;

        public EditorLibraryViewModel( MasterController parent, ILibrary library ) 
            : base(parent)
        {
            this.library = library;
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.groups = Cell.Create( from entry in library.Entries
                                       group entry by entry.Puzzle.Size into entryGroup
                                       select new Group( entryGroup.Key, entryGroup ) );
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }

        private void PerformBack()
        {
            Pop();
        }

        public Cell<IEnumerable<Group>> Groups
        {
            get
            {
                return groups;
            }
        }
        
        public class Group
        {
            private readonly Size size;

            private readonly IEnumerable<ILibraryEntry> entries;

            public Group(Size size, IEnumerable<ILibraryEntry> entries)
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

            public IEnumerable<Entry> Members
            {
                get
                {
                    return from entry in entries
                           select new Entry( entry );
                }
            }
        }

        public class Entry
        {
            private readonly ILibraryEntry entry;

            public Entry(ILibraryEntry entry)
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
