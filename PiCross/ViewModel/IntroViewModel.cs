using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cells;
using DataStructures;
using PiCross;
using ViewModel.Commands;

namespace ViewModel
{
    public class IntroViewModel : ViewModel
    {
        private readonly ICommand play;

        private readonly ICommand edit;

        private readonly ICommand quit;

        private readonly IStepwisePuzzleSolver logoSolver;

        private readonly IGrid<Cell<Square>> logo;

        private Scheduler scheduler;

        public IntroViewModel( MasterController parent )
            : base( parent )
        {
            play = EnabledCommand.FromDelegate( PerformPlay );
            edit = EnabledCommand.FromDelegate( PerformEdit );
            quit = EnabledCommand.FromDelegate( PerformQuit );

            logoSolver = CreateSolverForPuzzle( CreatePiCrossLogo() );
            logo = logoSolver.Grid.Map( ( Square square ) => Cell.Create( Square.FILLED ) ).Copy();
            //solvers = CreateLogoLetters().Select( CreateSolverForPuzzle ).ToList();            
            //logoLetters = solvers.Select( solver => solver.Grid.Map( ( Square square ) => Cell.Create( Square.FILLED ) ).Copy() ).ToList();
            SynchronizeLogoGridWithSolverGrid();

            ScheduleUpdate();
        }

        private IStepwisePuzzleSolver CreateSolverForPuzzle( Puzzle puzzle )
        {
            var result = Parent.PicrossFacade.CreateStepwisePuzzleSolver( columnConstraints: puzzle.ColumnConstraints, rowConstraints: puzzle.RowConstraints );

            return result;
        }

        private void SynchronizeLogoGridWithSolverGrid()
        {
            logo.Overwrite( logoSolver.Grid );
        }

        private void UpdateLogoGrid()
        {
            logoSolver.Step();

            SynchronizeLogoGridWithSolverGrid();
            ScheduleUpdate();
        }

        private void ScheduleUpdate()
        {
            scheduler = new Scheduler( .05, UpdateLogoGrid );
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
            return puzzle;
        }

        private Puzzle CreatePiCrossLogo()
        {
            var strings = new[] {
                "xxxxxxxxxx..............................",
                ".xx......x..............................",
                ".xx.....xx..............................",
                ".xx....xx...............................",
                ".xx...xx................................",
                ".xxxxxx..xxxxx..........................",
                ".xx.....xxxxx...........................",
                ".xx....xx...............................",
                ".xx.x..xx...............................",
                ".xx....xx...............................",
                ".xx.x.xx................................",
                ".xx.x.xx.......xx.xx....................",
                ".xx.x.xx.......xxxxx.........xxx...xxx..",
                ".xx.x.xx.......xxx..........xxxx..xxxx..",
                ".xx.x.xx.......xx....xxxx...xx....xx....",
                ".xx.x.xx.......xx...xx..xx..xxx....xx...",
                ".xx.x.xx.......xx...xx..xx...xxx...xxx..",
                ".xx.x.xx.......xx...xx..xx.....xx....xx.",
                ".xx.x..xxxxxxx.xx...xx..xx..xxxxx.xxxxx.",
                ".xx.x...xxxxxx.xx....xxxx...xxxx..xxxx.."
            };

            //var strings = new[] { ".xx.xx.",
            //                      "...x...",
            //                      "...x...",
            //                      ".xx.xx." };

            var puzzle = Puzzle.FromRowStrings( strings );

            //           Debug.Assert( puzzle.IsSolvable );

            return puzzle;
        }

        public IGrid<Cell<Square>> Logo
        {
            get
            {
                return logo;
            }
        }

        //public IEnumerable<IGrid<Cell<Square>>> LogoLetters
        //{
        //    get
        //    {
        //        return logoLetters;
        //    }
        //}

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
