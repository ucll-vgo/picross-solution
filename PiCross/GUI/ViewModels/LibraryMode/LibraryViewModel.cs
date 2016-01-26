using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels.PlayMode;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.IO;
using PiCross.Facade.Playing;
using PiCross.Game;

namespace GUI.ViewModels.LibraryMode
{
    public class LibraryViewModel : ViewModel
    {
        private readonly IPlayerProfile activeUser;

        private readonly ILibrary library;

        private readonly Cell<IList<GroupViewModel>> groups;

        private readonly ICommand back;

        private readonly ICommand filter;

        private readonly Cell<bool> showSolved;

        public LibraryViewModel( MasterController parent, ILibrary library, IPlayerProfile activeUser )
            : base( parent )
        {
            this.library = library;
            this.activeUser = activeUser;
            this.showSolved = Cell.Create( true );
            this.groups = Cell.Create<IList<GroupViewModel>>( null );
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.filter = EnabledCommand.FromDelegate( PerformFilter );
        }

        protected override void OnActivation()
        {
            base.OnActivation();

            this.groups.Value = ( from entry in library.Entries
                                  let puzzleInformation = activeUser.PuzzleInformation[entry.Puzzle]
                                  where !puzzleInformation.BestTime.Value.HasValue || showSolved.Value
                                  let entryVM = new LibraryEntryViewModel( entry, puzzleInformation, EnabledCommand.FromDelegate( () => PerformSelect( entry ) ) )
                                  group entryVM by entry.Puzzle.Size into entryGroup
                                  select new GroupViewModel( entryGroup.Key, entryGroup ) ).ToList();
        }

        public Cell<IList<GroupViewModel>> Groups
        {
            get
            {
                return this.groups;
            }
        }

        public ICommand Back
        {
            get
            {
                return back;
            }
        }

        public ICommand Filter
        {
            get
            {
                return filter;
            }
        }

        private void PerformBack()
        {
            Pop();
        }

        private void PerformSelect( ILibraryEntry entry )
        {
            var puzzle = entry.Puzzle;
            var playablePuzzle = PlayablePuzzle.Create( puzzle );
            var bestTime = activeUser.PuzzleInformation[puzzle].BestTime;

            Push( new PlayViewModel( Parent, playablePuzzle, bestTime ) );
        }

        private void PerformFilter()
        {
            Push( new FilterViewModel( this.Parent, showSolved ) );
        }

        public class GroupViewModel
        {
            private readonly Size size;

            private readonly IEnumerable<LibraryEntryViewModel> entries;

            public GroupViewModel( Size size, IEnumerable<LibraryEntryViewModel> entries )
            {
                this.size = size;
                this.entries = entries;
            }

            public IEnumerable<LibraryEntryViewModel> Entries
            {
                get
                {
                    return entries;
                }
            }

            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
    }

    public class LibraryEntryViewModel
    {
        private readonly IPlayerPuzzleInformationEntry userInfo;

        private readonly ILibraryEntry entry;

        private readonly IGrid<Cell<bool>> grid;

        private readonly ICommand select;

        public LibraryEntryViewModel( ILibraryEntry entry, IPlayerPuzzleInformationEntry userInfo, ICommand select )
        {
            this.entry = entry;
            this.userInfo = userInfo;
            this.grid = entry.Puzzle.Grid.Map( ( bool x ) => Cell.Create( x ) ).Copy();
            this.select = select;
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
                return userInfo.BestTime.Value.HasValue;
            }
        }

        public TimeSpan BestTime
        {
            get
            {
                return userInfo.BestTime.Value.HasValue ? userInfo.BestTime.Value.Value : TimeSpan.FromSeconds( -1 );
            }
        }

        public ICommand Select
        {
            get
            {
                return select;
            }
        }
    }
}
