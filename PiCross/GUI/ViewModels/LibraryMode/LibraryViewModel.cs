using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.IO;
using PiCross.Game;

namespace GUI.ViewModels.LibraryMode
{
    public class LibraryViewModel
    {
        private readonly ILibrary library;

        private readonly List<LibraryEntryViewModel> entries;

        public LibraryViewModel( ILibrary library )
        {
            this.library = library;
            this.entries = library.Entries.Select( x => new LibraryEntryViewModel( x ) ).ToList();
        }

        public IEnumerable<LibraryEntryViewModel> Entries
        {
            get
            {
                return this.entries;
            }
        }
    }

    public class LibraryEntryViewModel
    {
        private readonly ILibraryEntry entry;

        private readonly IGrid<Cell<bool>> grid;

        public LibraryEntryViewModel( ILibraryEntry entry )
        {
            this.entry = entry;
            this.grid = entry.Puzzle.Grid.Map( (bool x) => Cell.Create( x ) ).Copy();
        }

        public Puzzle Puzzle
        {
            get
            {
                return entry.Puzzle;
            }
        }
        
        public IGrid<Cell<bool>> Grid
        {
            get
            {
                return grid;
            }
        }
    }
}
