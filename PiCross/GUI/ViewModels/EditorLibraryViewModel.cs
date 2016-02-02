using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using Cells;
using DataStructures;
using PiCross.Facade.Editing;
using PiCross.Facade.IO;
using PiCross.Game;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
    public class EditorLibraryViewModel : ViewModel
    {
        private readonly ILibrary library;

        private readonly ICommand back;

        private readonly ICommand select;

        private readonly ICommand create;

        private readonly ObservableCollection<GroupViewModel> groups;

        public EditorLibraryViewModel( MasterController parent, ILibrary library )
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
            this.groups.Clear();

            foreach ( var group in from entry in library.Entries
                                   group entry by entry.Puzzle.Size )
            {
                var groupViewModel = FindGroupViewModelForSize( group.Key );

                groupViewModel.Update( group );
            }
        }

        private GroupViewModel FindGroupViewModelForSize(Size size)
        {
            // Look fore existing group
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
            var newGroupViewModel = new GroupViewModel( size, Enumerable.Empty<ILibraryEntry>() ) ;
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

            public GroupViewModel( Size size, IEnumerable<ILibraryEntry> entries )
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

            public void Update(IEnumerable<ILibraryEntry> entries)
            {
                foreach ( var entry in entries )
                {
                    Update( entry );
                }
            }

            private void Update(ILibraryEntry entry)
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
            private readonly ILibraryEntry entry;

            private readonly Cell<IGrid<Cell<bool>>> grid;

            public EntryViewModel( ILibraryEntry entry )
            {
                this.entry = entry;
                this.grid = Cell.Create( entry.Puzzle.Grid.Map( ( bool x ) => Cell.Create( x ) ).Copy() );
            }

            public Cell<IGrid<Cell<bool>>> Grid
            {
                get
                {
                    return grid;
                }
            }

            public ILibraryEntry LibraryEntry
            {
                get
                {
                    return entry;
                }
            }

            public void Update()
            {
                // TODO!
            }
        }
    }
}
