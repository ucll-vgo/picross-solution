#!/usr/bin/env python

from functools import reduce
import argparse


class Grid:
    def __init__(self, width, height, initializer):
        self.__data = [ [ initializer(x, y) for x in range(width) ] for y in range(height) ]

    def __getitem__(self, position):
        x, y = position
        return self.__data[y][x]

    def __setitem__(self, position, value):
        x, y = position
        self.__data[y][x] = value

    def column(self, i):
        return Column(self, i)

    def row(self, i):
        return Row(self, i)

    @property
    def rows(self):
        return [ self.row(i) for i in range(self.height) ]

    @property
    def columns(self):
        return [ self.column(i) for i in range(self.width) ]

    @property
    def width(self):
        return len(self.__data[0])

    @property
    def height(self):
        return len(self.__data)

    def count(self, predicate):
        return len([1 for row in self.__data for elt in row if predicate(elt)])


class Column:
    def __init__(self, parent, index):
        self.__parent = parent
        self.__index = index

    def __getitem__(self, i):
        return self.__parent[self.__index, i]

    def __setitem__(self, i, value):
        self.__parent[self.__index, i] = value

    def __len__(self):
        return self.__parent.height

    def __iter__(self):
        for i in range(len(self)):
            yield self[i]

class Row:
    def __init__(self, parent, index):
        self.__parent = parent
        self.__index = index

    def __getitem__(self, i):
        return self.__parent[i, self.__index]

    def __setitem__(self, i, value):
        self.__parent[i, self.__index] = value

    def __len__(self):
        return self.__parent.width

    def __iter__(self):
        for i in range(len(self)):
            yield self[i]



EMPTY = 0
FILLED = 1
UNKNOWN = -1

def generate_combinations(width, ns):
    def aux(width, ns):
        if width >= 0:
            if ns:
                n, *ns = ns
                yield from ( [ EMPTY ] * k + [ FILLED ] * n + [ EMPTY ] + rest for k in range(0, width + 1) for rest in aux(width - n - k - 1, ns) )
            else:
                yield [ EMPTY ] * width

    return ( ns[:-1] for ns in aux(width + 1, ns) )


def compatible(bs, cs):
    def aux(b, c):
        return b == UNKNOWN or c == UNKNOWN or b == c

    return all( aux(b, c) for b, c in zip(bs, cs) )


def generate_compatible(bs, ns):
    return ( cs for cs in generate_combinations(len(bs), ns) if compatible(bs, cs) )


def merge(bs, cs):
    def aux(b, c):
        return UNKNOWN if b != c else b

    for i in range(len(bs)):
        bs[i] = aux(bs[i], cs[i])


def improve_sequence(bs, ns):
    first, *rest = list(generate_compatible(bs, ns))

    for i, x in enumerate(first):
        bs[i] = x

    for cs in rest:
        merge(bs, cs)

def improve(grid, column_constraints, row_constraints):
    def improve_column(i):
        improve_sequence(grid.column(i), column_constraints[i])

    def improve_row(i):
        improve_sequence(grid.row(i), row_constraints[i])

    def improve_columns():
        for i in range(grid.width):
            improve_column(i)

    def improve_rows():
        for i in range(grid.height):
            improve_row(i)

    improve_rows()
    improve_columns()


def solve_puzzle(column_constraints, row_constraints):
    width = len(column_constraints)
    height = len(row_constraints)
    grid = Grid(width, height, lambda x, y: UNKNOWN)

    def count_unknowns():
        return grid.count(lambda x: x == UNKNOWN)

    previous_count = -1
    current_count = count_unknowns()

    while previous_count != current_count:
        improve(grid, column_constraints, row_constraints)
        previous_count = current_count
        current_count = count_unknowns()

    return grid


def show(grid):
    def aux(b):
        if b == UNKNOWN:
            return '?'
        elif b == EMPTY:
            return '.'
        else:
            return 'x'

    return '\n'.join( ''.join( aux(c) for c in row ) for row in grid.rows )


def derive_constraints(grid):
    def derive(bs):
        def aux(ns, acc, bs):
            if bs:
                b, *bs = bs

                if b == EMPTY:
                    return aux( [*ns, acc], 0, bs )
                else:
                    return aux( ns, acc + 1, bs )
            else:
                return [ *ns, acc ]

        return [ x for x in aux([], 0, bs) if x != 0 ]

    column_constraints = [ derive(list(column)) for column in grid.columns ]
    row_constraints = [ derive(list(row)) for row in grid.rows ]

    return ( column_constraints, row_constraints )


def read_constraints_from_file(filename):
    with open(filename) as file:
        width, height = map(int, file.readline().split(' '))

        column_constraints = [ list(map(int, file.readline().split(' '))) for _ in range(width) ]
        row_constraints = [ list(map(int, file.readline().split(' '))) for _ in range(height) ]

        return (column_constraints, row_constraints)


def read_image_from_file(filename):
    with open(filename) as file:
        rows = [ line.strip() for line in file ]
        width = len(rows[0])
        height = len(rows)

        return Grid(width, height, lambda x, y: EMPTY if rows[y][x] == '.' else FILLED)


def process_command_line_arguments():
    def solve_command(args):
        column_constraints, row_constraints = read_constraints_from_file(args.filename)
        print(show(solve_puzzle(column_constraints, row_constraints)))

    def derive_constraints_command(args):
        filename = args.filename
        grid = read_image_from_file(filename)
        column_constraints, row_constraints = derive_constraints(grid)
        print(f'{grid.width} {grid.height}')
        print('\n'.join( ' '.join( str(n) for n in ns ) for ns in column_constraints ))
        print('\n'.join( ' '.join( str(n) for n in ns ) for ns in row_constraints ))


    parser = argparse.ArgumentParser(prog='picross')
    parser.set_defaults(func=lambda args: parser.print_help())
    subparsers = parser.add_subparsers(help='sub-command help')

    solve_parser = subparsers.add_parser('solve', help='solves PiCross puzzle')
    solve_parser.add_argument('filename', help='file containing constraints', action='store')
    solve_parser.set_defaults(func=solve_command)

    solve_parser = subparsers.add_parser('constraints', help='derives constraints from image')
    solve_parser.add_argument('filename', help='file containing image', action='store')
    solve_parser.set_defaults(func=derive_constraints_command)

    args = parser.parse_args()
    args.func(args)


process_command_line_arguments()