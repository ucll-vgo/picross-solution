using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using Cells;
using DataStructures;
using PiCross.Facade;
using PiCross.Game;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GUI.ViewModels
{
    public class EditorLibraryViewModel : ViewModel
    {
        private readonly IPuzzleLibrary library;

        private readonly ICommand back;

        private readonly ICommand select;

        private readonly ICommand create;

        private readonly ObservableCollection<GroupViewModel> groups;

        public EditorLibraryViewModel( MasterController parent, IPuzzleLibrary library )
            : base( parent )
        {
            this.library = library;
            this.back = EnabledCommand.FromDelegate( PerformBack );
            this.select = EnabledCommand.FromDelegate<EntryViewModel>( PerformSelect );
            this.create = EnabledCommand.FromDelegate<string>( PerformCreate );
            this.groups = new ObservableCollection<GroupViewModel>();
        }

        protected override void OnActivation()
        {
            UpdateGroups();
        }

        private void UpdateGroups()
        {
            foreach ( var group in from entry in library.Entries
                                   group entry by entry.Puzzle.Size )
            {
                var groupViewModel = FindGroupViewModelForSize( group.Key );

                groupViewModel.Update( group );
            }
        }

        private GroupViewModel FindGroupViewModelForSize(Size size)
        {
            // Look for existing group
            foreach ( var groupViewModel in groups )
            {
                if ( groupViewModel.Size == size )
                {
                    return groupViewModel;
                }
            }

            // No group found, find insertion index
            var index = 0;
            while ( index < groups.Count && SizeComesBefore( groups[index].Size, size ) )
            {
                ++index;
            }

            // Create new group and insert it
            var newGroupViewModel = new GroupViewModel( size, Enumerable.Empty<IPuzzleLibraryEntry>() ) ;
            groups.Insert( index, newGroupViewModel );

            return newGroupViewModel;
        }

        private bool SizeComesBefore(Size sizeA, Size sizeB)
        {
            if ( sizeA.Width < sizeB.Width )
            {
                return true;
            }
            else if ( sizeA.Width > sizeB.Height )
            {
                return false;
            }
            else if ( sizeA.Height < sizeB.Height )
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public ICommand Create
        {
            get
            {
                return create;
            }
        }

        private void PerformBack()
        {
            Pop();
        }

        private void PerformSelect( EntryViewModel entry )
        {
            Push( new EditorViewModel( Parent, entry.LibraryEntry ) );
        }

        private void PerformCreate( string sizeString )
        {
            var match = Regex.Match( sizeString, @"^(\d+)x(\d+)$" );

            if ( !match.Success )
            {
                throw new ArgumentException( "BUG" );
            }
            else
            {
                var width = int.Parse( match.Groups[1].Value );
                var height = int.Parse( match.Groups[2].Value );
                var size = new Size( width, height );
                var puzzle = Puzzle.CreateEmpty( size );

                var newEntry = library.Create( puzzle, "unnamed" );

                UpdateGroups();
            }
        }

        public ObservableCollection<GroupViewModel> Groups
        {
            get
            {
                return groups;
            }
        }

        public class GroupViewModel
        {
            private readonly Size size;

            private readonly ObservableCollection<EntryViewModel> entries;

            public GroupViewModel( Size size, IEnumerable<IPuzzleLibraryEntry> entries )
            {
                this.size = size;
                this.entries = new ObservableCollection<EntryViewModel>( entries.Select( e => new EntryViewModel( e ) ) );
            }

            public Size Size
            {
                get
                {
                    return size;
                }
            }

            public ObservableCollection<EntryViewModel> Members
            {
                get
                {
                    return entries;
                }
            }

            public void Update(IEnumerable<IPuzzleLibraryEntry> entries)
            {
                foreach ( var entry in entries )
                {
                    Update( entry );
                }
            }

            private void Update(IPuzzleLibraryEntry entry)
            {
                // Find corresponding view model
                foreach ( var entryViewModel in this.entries )
                {
                    if ( entryViewModel.LibraryEntry == entry )
                    {
                        entryViewModel.Update();
                        return;
                    }
                }

                // No corresponding view model found
                var viewModel = new EntryViewModel( entry );
                this.entries.Add( viewModel );
            }
        }

        public class EntryViewModel
        {
            private readonly IPuzzleLibraryEntry entry;

            private readonly IGrid<Cell<bool>> grid;

            public EntryViewModel( IPuzzleLibraryEntry entry )
            {
                this.entry = entry;
                this.grid = entry.Puzzle.Grid.Map( ( pos, val ) => Cell.Derived( () => entry.Puzzle.Grid[pos] ) );
            }

            public IGrid<Cell<bool>> Grid
            {
                get
                {
                    return grid;
                }
            }

            public IPuzzleLibraryEntry LibraryEntry
            {
                get
                {
                    return entry;
                }
            }

            public void Update()
            {
                grid.ForEach<Cell<bool>>( cell => cell.Refresh() );
            }
        }
    }
}
