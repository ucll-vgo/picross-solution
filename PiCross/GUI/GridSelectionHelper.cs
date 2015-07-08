using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiCross.Cells;
using PiCross.DataStructures;

namespace GUI
{
    public class GridSelectionHelper
    {
        private readonly IGrid<DeferredCell<int?>> grid;

        private readonly IGrid<Cell<int?>> readonlyView;

        private Vector2D selectionStart;

        private Vector2D selectionEnd;

        public GridSelectionHelper( Size size )
        {
            this.grid = Grid.Create( size, _ => new DeferredCell<int?>( null ) );
            this.readonlyView = this.grid.Map( x => x.AsReadOnly() ).Copy();
            this.selectionStart = null;
            this.selectionEnd = null;
        }

        public IGrid<Cell<int?>> Selection
        {
            get
            {
                return readonlyView;
            }
        }

        public Vector2D SelectionStart
        {
            get
            {
                return selectionStart;
            }
            set
            {
                selectionStart = value;

                OnSelectionChanged();
            }
        }

        public Vector2D SelectionEnd
        {
            get
            {
                return selectionEnd;
            }
            set
            {
                selectionEnd = value;

                OnSelectionChanged();
            }
        }

        private void OnSelectionChanged()
        {
            RecomputeGrid();
            BroadcastChanges();
        }

        private void BroadcastChanges()
        {
            foreach ( var position in grid.AllPositions )
            {
                grid[position].BroadcastChange();
            }
        }

        private void RecomputeGrid()
        {
            ResetGrid();

            if ( selectionStart != null && selectionEnd != null )
            {
                var delta = selectionEnd - selectionStart;
                var absDX = Math.Abs( delta.X );
                var absDY = Math.Abs( delta.Y );

                if ( absDX <= absDY )
                {
                    MakeVerticalSelection( selectionStart, delta.Y );
                }
                else
                {
                    MakeHorizontalSelection( selectionStart, delta.X );
                }
            }
        }

        private void ResetGrid()
        {
            foreach ( var position in grid.AllPositions )
            {
                grid[position].Value = null;
            }
        }

        private void MakeVerticalSelection( Vector2D start, int dy )
        {
            var step = Math.Sign( dy );
            var nsteps = Math.Abs( dy ) + 1;
            var delta = new Vector2D( 0, step );

            MakeSelection( start: start, nsteps: nsteps, delta: delta );
        }

        private void MakeHorizontalSelection( Vector2D start, int dx )
        {
            var step = Math.Sign( dx );
            var nsteps = Math.Abs( dx ) + 1;
            var delta = new Vector2D( step, 0 );

            MakeSelection( start: start, nsteps: nsteps, delta: delta );
        }

        private void MakeSelection( Vector2D start, int nsteps, Vector2D delta )
        {
            var current = start;

            for ( var i = 0; i != nsteps; ++i )
            {
                grid[current].Value = i;

                current += delta;
            }
        }
    }
}
