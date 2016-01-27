using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using GUI.ViewModels.LibraryMode;
using GUI.ViewModels.PlayerSelection;
using PiCross.Cells;
using PiCross.DataStructures;
using PiCross.Facade.IO;
using PiCross.Game;

namespace GUI.ViewModels.Intro
{
    public class IntroViewModel : ViewModel
    {
        private readonly ICommand play;

        private readonly ICommand edit;

        private readonly ICommand quit;

        private readonly StepwiseSolver solver;

        private readonly IGrid<Cell<Square>> logoGrid;

        private Scheduler scheduler;

        public IntroViewModel( MasterController parent )
            : base( parent )
        {
            play = EnabledCommand.FromDelegate( PerformPlay );
            edit = EnabledCommand.FromDelegate( PerformEdit );
            quit = EnabledCommand.FromDelegate( PerformQuit );

            var puzzle = CreatePiCrossLogo();
            var solverGrid = SolverGrid.FromPuzzle( puzzle );
            solver = new StepwiseSolver( solverGrid );
            logoGrid = solverGrid.Squares.Map( ( Square square ) => Cell.Create( Square.FILLED ) ).Copy();
            SynchronizeLogoGridWithSolverGrid();

            ScheduleUpdate();
        }

        private void SynchronizeLogoGridWithSolverGrid()
        {
            foreach ( var position in logoGrid.AllPositions )
            {
                logoGrid[position].Value = solver.Grid.Squares[position];
            }
        }

        private void UpdateLogoGrid()
        {
            solver.Step();
            SynchronizeLogoGridWithSolverGrid();

            ScheduleUpdate();
        }

        private void ScheduleUpdate()
        {
            scheduler = new Scheduler( 1, UpdateLogoGrid );
        }

        private Puzzle CreatePiCrossLogo()
        {
            //var strings = new[] { ".................................",
            //                      "xxxx.....xxx.....................",
            //                      "x...x...x........................",
            //                      "x...x.x.x........................",
            //                      "xxxx....x....x.xx..xx...xxx..xxx.",
            //                      "x.....x.x....xx...x..x.x....x....",
            //                      "x.....x.x....x....x..x..xx...xx..",
            //                      "x.....x.x....x....x..x....x....x.",
            //                      "x.....x..xxx.x.....xx..xxx..xxx..",
            //                      "................................." };

            var strings = new[] { ".xx.xx.",
                                  "...x...",
                                  "...x...",
                                  ".xx.xx." };

            var puzzle = Puzzle.FromRowStrings( strings );

            Debug.Assert( puzzle.IsSolvable );

            return puzzle;
        }

        public IGrid<Cell<Square>> LogoGrid
        {
            get
            {
                return logoGrid;
            }
        }

        public ICommand Play
        {
            get
            {
                return play;
            }
        }

        public ICommand Edit
        {
            get
            {
                return edit;
            }
        }

        public ICommand Quit
        {
            get
            {
                return quit;
            }
        }

        private void PerformPlay()
        {
            Push( new PlayerSelectionViewModel( Parent ) );
        }

        private void PerformEdit()
        {
            Push( new EditorLibraryViewModel( Parent, Parent.Library ) );
        }

        private void PerformQuit()
        {
            Pop();
        }

        public override void OnTick( double dt )
        {
            scheduler.Tick( dt );
        }
    }
}
