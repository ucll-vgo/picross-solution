using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using PiCross;

namespace PiCon
{
    class Program
    {
        static void Main( string[] args )
        {
            var dimensions = ReadInts();
            var width = dimensions[0];
            var height = dimensions[1];

            var columnConstraints = new List<Constraints>();
            var rowConstraints = new List<Constraints>();

            for ( var i = 0; i != width; ++i )
            {
                var ns = ReadInts();
                columnConstraints.Add( Constraints.FromValues( ns ) );
            }

            for ( var i = 0; i != height; ++i )
            {
                var ns = ReadInts();
                rowConstraints.Add( Constraints.FromValues( ns ) );
            }

            var puzzle = Puzzle.FromConstraints( columnConstraints: Sequence.FromEnumerable( columnConstraints ), rowConstraints: Sequence.FromEnumerable( rowConstraints ) );

            Console.WriteLine( "Woumpousse" );
            Console.WriteLine( "{0} {1}", width, height );

            foreach ( var row in puzzle.Grid.Rows )
            {
                Console.WriteLine( row.Map( b => b ? 'x' : '.' ).AsString() );
            }
        }

        private static int[] ReadInts()
        {
            return Console.ReadLine().Split( ' ' ).Select( int.Parse ).ToArray();
        }
    }
}
