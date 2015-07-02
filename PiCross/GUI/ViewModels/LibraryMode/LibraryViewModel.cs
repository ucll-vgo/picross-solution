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
    public class LibraryViewModel : ViewModel
    {
        private readonly IPlayerProfile activeUser;

        private readonly ILibrary library;

        private readonly List<LibraryEntryViewModel> entries;

        public LibraryViewModel( MasterController parent, ILibrary library, IPlayerProfile activeUser )
            : base( parent )
        {
            this.library = library;
            this.activeUser = activeUser;
            this.entries = library.Entries.Select( x => new LibraryEntryViewModel( x, activeUser.PuzzleInformation[x.Puzzle] ) ).ToList();
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
        private readonly IPlayerPuzzleInformationEntry userInfo;

        private readonly ILibraryEntry entry;

        private readonly IGrid<Cell<bool>> grid;

        public LibraryEntryViewModel( ILibraryEntry entry, IPlayerPuzzleInformationEntry userInfo )
        {
            this.entry = entry;
            this.userInfo = userInfo;
            this.grid = entry.Puzzle.Grid.Map( ( bool x ) => Cell.Create( x ) ).Copy();
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

        public bool Solved
        {
            get
            {
                return userInfo != null;
            }
        }
    }
}
