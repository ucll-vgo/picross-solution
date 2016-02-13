using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Commands;
using Cells;
using DataStructures;
using PiCross;

namespace GUI.ViewModels
{
    public class IntroViewModel : ViewModel
    {
        private readonly ICommand play;

        private readonly ICommand edit;

        private readonly ICommand quit;

        private readonly IList<IStepwisePuzzleSolver> solvers;

        private readonly IList<IGrid<Cell<Square>>> logoLetters;

        private Scheduler scheduler;

        public IntroViewModel( MasterController parent )
            : base( parent )
        {
            play = EnabledCommand.FromDelegate( PerformPlay );
            edit = EnabledCommand.FromDelegate( PerformEdit );
            quit = EnabledCommand.FromDelegate( PerformQuit );

            solvers = CreateLogoLetters().Select( CreateSolverForPuzzle ).ToList();
            logoLetters = solvers.Select( solver => solver.Grid.Map( ( Square square ) => Cell.Create( Square.FILLED ) ).Copy() ).ToList();
            SynchronizeLogoGridWithSolverGrid();

            ScheduleUpdate();
        }

        private IStepwisePuzzleSolver CreateSolverForPuzzle(Puzzle puzzle)
        {
            var result = Parent.PicrossFacade.CreateStepwisePuzzleSolver( columnConstraints: puzzle.ColumnConstraints, rowConstraints: puzzle.RowConstraints );

            return result;
        }

        private void SynchronizeLogoGridWithSolverGrid()
        {
            for ( var i = 0; i != solvers.Count; ++i )
            {
                logoLetters[i].Overwrite( solvers[i].Grid );
            }
        }

        private void UpdateLogoGrid()
        {
            foreach ( var solver in solvers )
            {
                solver.Step();
            }

            SynchronizeLogoGridWithSolverGrid();
            ScheduleUpdate();
        }

        private void ScheduleUpdate()
        {
            scheduler = new Scheduler( .5, UpdateLogoGrid );
        }

        private IEnumerable<Puzzle> CreateLogoLetters()
        {
            yield return CreateLogoLetterP();
            yield return CreateLogoLetterI();
            yield return CreateLogoLetterC();
            yield return CreateLogoLetterR();
            yield return CreateLogoLetterO();
            yield return CreateLogoLetterS();
            yield return CreateLogoLetterS();
        }

        private Puzzle CreateLogoLetterP()
        {
            var rows = new[] {
                "xxxx.",
                "x...x",
                "x...x",
                "xxxx.",
                "x....",
                "x....",
                "x....",
                "x....",
            };

            var puzzle = Puzzle.FromRowStrings( rows );
            Debug.Assert( puzzle.IsSolvable );
            return puzzle;
        }

        private Puzzle CreateLogoLetterI()
        {
            var rows = new[] {
                ".....",
                "..x..",
                ".....",
                ".xx..",
                "..x..",
                "..x..",
                "..x..",
                ".xxx.",
            };

            var puzzle = Puzzle.FromRowStrings( rows );
            Debug.Assert( puzzle.IsSolvable );
            return puzzle;
        }

        private Puzzle CreateLogoLetterC()
        {
            var rows = new[] {
                "xxxxx",
                "x....",
                "x....",
                "x....",
                "x....",
                "x....",
                "x....",
                "xxxxx",
            };

            var puzzle = Puzzle.FromRowStrings( rows );
            Debug.Assert( puzzle.IsSolvable );
            return puzzle;
        }
        private Puzzle CreateLogoLetterR()
        {
            var rows = new[] {
                ".....",
                ".....",
                ".....",
                "xxx..",
                "x..x.",
                "xxx..",
                "x..x.",
                "x..x.",
            };

            var puzzle = Puzzle.FromRowStrings( rows );
            Debug.Assert( puzzle.IsSolvable );
            return puzzle;
        }

        private Puzzle CreateLogoLetterO()
        {
            var rows = new[] {
                ".....",
                ".....",
                ".....",
                "xxxxx",
                "x...x",
                "x...x",
                "x...x",
                "xxxxx",
            };

            var puzzle = Puzzle.FromRowStrings( rows );
            Debug.Assert( puzzle.IsSolvable );
            return puzzle;
        }

        private Puzzle CreateLogoLetterS()
        {
            var rows = new[] {
                ".....",
                ".....",
                ".....",
                "xxxxx",
                "x....",
                "xxxxx",
                "....x",
                "xxxxx",
            };

            var puzzle = Puzzle.FromRowStrings( rows );
            Debug.Assert( puzzle.IsSolvable );
            return puzzle;
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

            //           Debug.Assert( puzzle.IsSolvable );

            return puzzle;
        }

        public IEnumerable<IGrid<Cell<Square>>> LogoLetters
        {
            get
            {
                return logoLetters;
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
            Push( new EditorLibraryViewModel( Parent, Parent.GameData.PuzzleLibrary ) );
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
