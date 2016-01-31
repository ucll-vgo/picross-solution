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

            UpdateGroups();
        }

        private void UpdateGroups()
        {
            this.groups.Clear();

            foreach ( var groupViewMode in from entry in library.Entries
                                           group entry by entry.Puzzle.Size into entryGroup
                                           select new GroupViewModel( entryGroup.Key, entryGroup ) )
            {
                this.groups.Add( groupViewMode );
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

                var newEntry = library.Create( size, "unnamed" );

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
        }

        public class EntryViewModel
        {
            private readonly ILibraryEntry entry;

            public EntryViewModel( ILibraryEntry entry )
            {
                this.entry = entry;
            }

            public IGrid<Cell<bool>> Grid
            {
                get
                {
                    return entry.Puzzle.Grid.Map( ( bool x ) => Cell.Create( x ) ).Copy();
                }
            }

            public ILibraryEntry LibraryEntry
            {
                get
                {
                    return entry;
                }
            }
        }
    }
}
