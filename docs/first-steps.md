# Getting started

## PiCross

PiCross puzzles are also known as *nonograms*.
First, you should familiarize yourself with the game: read the [rules](https://en.wikipedia.org/wiki/Nonogram)
and solve some puzzles online:

* https://www.nonograms.org/
* https://www.puzzle-nonograms.com/
* https://www.hanjie-star.com/

Let's introduce some terminology:

* The central part of the puzzle is called the *grid*.
* The grid is made out of *squares*.
* The numbers on the sides of the grid are called the *constraints*.
  The numbers on the left are the *row constraints*, the numbers on the top
  are the *column constraints*.

## `Puzzle` Class

The first thing we'll want to implement is a working playable puzzle, i.e. focus on creating a GUI that
allows people to play a puzzle and ignore all the other functionality the domain offers.

First, we need a puzzle to solve. A puzzle is modeled by the `Puzzle` class.
A `Puzzle` object contains all information about a puzzle. The most important members are

* the `Grid` property: this grid contains the actual solution of the puzzle. It has type
 `IGrid<bool>`, where `true` values represent filled squares and `false` empty squares.
* the properties `RowConstraints` and `ColumnConstraints` represent the constraints of the puzzle.
  The type of both of these properties is `ISequence<Constraints>`. `ISequence<T>` is quite similar to
  arrays/lists, but with some added functionality, which we won't get into right now.

A `Puzzle` object is *immutable*, which means that once created, it can never be changed.

## Creating a `Puzzle` Object

The `Puzzle` class sports multiple static factory methods which you can use to create a puzzle,
either from constraints using `Puzzle.FromConstraints` or from the solution using `Puzzle.FromRowStrings`.
For example,

```C#
var puzzle = Puzzle.FromRowStrings(
    "xxxxx",
    "x...x",
    "x...x",
    "x...x",
    "xxxxx"
)
```

creates a puzzle whose solution is a 5&times;5 square.

## Creating a Playable Puzzle

The `Puzzle` class does not provide any functionality to interact with a puzzle.
Instead, it just contains the data describing the puzzle.

* A `Puzzle` is immutable. It would be a rather absurd game if the player
  is not able to do anything.
* A `Puzzle`'s Grid contains `bool`s: either a square is filled, or it is empty.
  To solve a puzzle, we also want an "as of yet unknown" value. When
  starting to solve a puzzle, the entire grid should be filled with this `unknown` value,
  and the player then completes the puzzle by gradually marking
  certain squares as empty or filled.

The domain offers a separate class called `PlayablePuzzle`.
`PlayablePuzzle` objects offer a *modifiable* grid of type
`IGrid<IPlayablePuzzleSquare>`. Each square in this grid can contain one of *three* possible values:
`Square.UNKNOWN`, `Square.FILLED` or `Square.EMPTY`.

We encourage you to take a look through the domain code and find this `PlayablePuzzle` class. However,
you'll find that it is declared `internal`, i.e. it is private to the domain. What now?

We endeavored to minimize the number of domain classes
you can instantiate directly. The reason is that once we allow you to create
a `PlayablePuzzle` directly, this takes a lot of freedom away from us domain authors:
the `PlayablePuzzle` class cannot be renamed without breaking your code,
we cannot redesign it, etc. But, admittedly, making it `internal` makes it rather useless, as
you cannot access it at all.

A solution to this problem consists relying on interfaces. The
interface makes certain guarantees about what functionality we will provide,
so that at least you can actually intereact with it. On our side, we can do all kinds
of crazy stuff: we can have this interface implemented by any class we want, we
can spread it out over multiple classes, etc. So, using interfaces gives you promises of functionality
and gives us freedom of design. It's a win win situation!

Well, almost. Interfaces have a limitation: they cannot be instantiated.
To remedy this, we introduce factory methods, available in the `PiCrossFacade` class:

```c#
var puzzle = Puzzle.FromRowStrings(
    "xxxxx",
    "x...x",
    "x...x",
    "x...x",
    "xxxxx"
);
var facade = new PiCrossFacade();
var playablePuzzle = facade.CreatePlayablePuzzle(puzzle);
```

Here, `CreatePlayablePuzzle` wraps `puzzle` inside a `IPlayablePuzzle` object.

## Using a Playable Puzzle

Let's take a look at this `IPlayablePuzzle` interface. It contains only a few members:

* `Grid` represents the current state of the grid.
  This is what the player interacts with: (s)he's supposed to be able to
  change an arbitrary square of the grid to filled, empty (or back to unknown.)
* `ColumnConstraints` and `RowConstraints` represent the constraints.
* `IsSolved` is a `Cell<bool>`. If `true`, it means the grid, in its current state, contains
  the correct solution, i.e. the puzzle is solved.
  `false` means the opposite.

The `Grid` and both `Constraints` properties are actually upgraded version
of their `Puzzle` counterparts: they all rely heavily on `Cell`s, which
makes it easy for you to bind your GUI controls to them. They also
offer extra information such as `IsSatisfied` in `IPlayablePuzzleConstraints` and
`IPlayablePuzzleConstraintsValue`.
You should take a quick peek at `IPlayablePuzzle` and the related interfaces
to get an idea of what functionality they offer.

## Visualizing a Playable Puzzle

If there's anything you need to learn about software development, it's this: baby steps.
We're serious. Don't try to create the entire GUI in one go,
because if it doesn't work, you won't know where to look for mistakes.

Let's start with visualizing the puzzle. This is probably the most
complicated part of developing PiCross, and unfortunately, we have
little choice but to start with it. To alleviate your pain, we
have written a `PiCrossControl` for you.

### Step 1: Red Rectangles

To learn to work with it, careful experimentation is key. Let's start
with adding a `PiCrossControl` to our `MainWindow`:

```diff
  <Window x:Class="View.MainWindow"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
          xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
+         xmlns:controls="clr-namespace:View.Controls"
          mc:Ignorable="d"
          Title="MainWindow" Height="350" Width="525">
      <Grid>
+         <controls:PiCrossControl>
+         </controls:PiCrossControl>
      </Grid>
  </Window>
```

`PiCrossControl` cannot magically know what to show. We need to give it
some data. In WPF, this is geneerally done using dependency properties, so
let's explore what properties `PiCrossControl` has to offer. For this, you
can either take a look at its source code or use the XAML Designer help you.

The `Grid` property allows you to tell `PiCrossControl` which grid
to draw. The property's type is `IGrid<object>`, which means
you can pass along any object you wish. This raises the question:
how can `PiCrossControl` know how to draw that object?

`SquareTemplate` seems like an interesting property: it tells `PiCrossControl` how
to draw each square in the `Grid`. It looks as if we're now ready to get
something to appear on our screens.

First, we need a `IGrid<object>`. We can make one using `Grid.Create`.

```diff
  // Using declarations
+ using Grid = DataStructures.Grid;
+ using Size = DataStructures.Size;

  namespace View
  {
      /// <summary>
      /// Interaction logic for MainWindow.xaml
      /// </summary>
      public partial class MainWindow : Window
      {
          public MainWindow()
          {
              InitializeComponent();

+             var grid = Grid.Create<string>( new Size( 5, 5 ), p => "x" );
          }
      }
  }
```

Both WPF and our code define `Grid` and `Size`. If we were to simply use
`Grid` and `Size` in our code, the compiler would not know which one
we meant. The `using` declarations at the top of the file
resolve this ambiguity: it effectly tells the compiler
that whenever you write `Grid`, you men `DataStructures.Grid`.
Idem for `Size`.

The line added to `MainWindow`'s constructor creates a 5&times;5 grid
filled with `"x"`. Understanding the second parameter
is not important, but for those interested: it's an anonymous function
that given a parameter `p` (of type `Vector2D`, which is inferred by the compiler)
returns `"x"`.

So, now we've got a 5&times;5 grid filled with `"x"`es. It's a good enough start.
We'll fill it with more interesting values later on.
Let's focus now on finding a way to pass this grid along to our `PiCrossControl`.
The easiest way to achieve this is to give the control a name:

```diff
  <Grid>
-     <controls:PiCrossControl>
+     <controls:PiCrossControl x:Name="picrossControl">
      </controls:PiCrossControl>
  </Grid>
```

and to programmatically set its `Grid` property:

```diff
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var grid = Grid.Create<string>( new Size( 5, 5 ), p => "x" );
+           picrossControl.Grid = grid;
        }
    }
```

Next, let's define a `SquareTemplate`.

```diff
  <Grid>
      <controls:PiCrossControl>
+         <controls:PiCrossControl.SquareTemplate>
+             <DataTemplate>
+                 <Rectangle Width="32" Height="32" Fill="Red" Stroke="Black" />
+             </DataTemplate>
+         </controls:PiCrossControl.SquareTemplate>
      </controls:PiCrossControl>
  </Grid>
```

Running your project should make a window appear with 5&times;5 red rectangles. Make sure
you understand why there are 25 such rectangles. Feel free to experiment a bit (e.g. change the rectangle's color
or grid's size) to verify your assumptions.

### Step 2: DataContexts

Every square is now drawn the same, i.e., as a red rectangle. For our game to be playable,
each square has to be able to adapt its looks depending on the state of the game. In the case of PiCross,
squares can have one of three states: empty, filled or unknown. The `SquareTemplate` needs
to be able to access that information and draw itself accordingly.

As with other WPF-controls relying on templates, we will rely on `DataContext`s to pass along information.
The `PiCrossControl` was given a `Grid` which right now contains nothing but `"x"`s. For each element
of the `Grid`, the `PiCrossControl` instantiates the `SquareTemplate` and sets its `DataContext` to
the corresponding element. Using bindings we can access the data stored in this `DataContext`.

Right now, we ignore the `"x"` value completely. Let's make it appear.
Instead of a `Rectangle`, we'll use a `TextBlock` whose `Text` property
is bound to the `Grid`'s corresponding value.

```diff
    <Grid>
        <controls:PiCrossControl x:Name="picrossControl">
            <controls:PiCrossControl.SquareTemplate>
                <DataTemplate>
-                   <Rectangle Width="32" Height="32" Fill="Red" Stroke="Black" />
+                   <TextBlock Width="32" Height="32" Background="Red" Text="{Binding}" />
                </DataTemplate>
            </controls:PiCrossControl.SquareTemplate>
        </controls:PiCrossControl>
    </Grid>
```

`{Binding}` means "take the value of the `DataContext` itself." Since
the `DataContext` always equals `"x"`, regardless of which square is being processed,
each `TextBlock`'s `Text` property should be set to `"x"`. You can verify this by launching
the application: a 5&times;5 grid of `x`'s should appear.

If this works, we know we have successfully accessed the `DataContext`. We can now
take the next step: make the `DataContext` different for each square.

### Step 3: Coordinates

We created our grid as follows:

```c#
var grid = Grid.Create<string>( new Size( 5, 5 ), p => "x" );
```

Instead of having each grid square be equal to `"x"`, let's have
it show the square's coordinates:

```diff
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

-           var grid = Grid.Create<string>( new Size( 5, 5 ), p => "x" );
+           var grid = Grid.Create<string>( new Size( 5, 5 ), p => p.ToString() );
            picrossControl.Grid = grid;
        }
    }
```

Run the application to verify that the `x`s have indeed been replaced by coordinates.

### Step 4: Puzzles

Let's now switch to showing an actual puzzle.

```diff
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

-           var grid = Grid.Create<string>( new Size( 5, 5 ), p => p.ToString() );
-           picrossControl.Grid = grid;

+           var puzzle = Puzzle.FromRowStrings(
+               "xxxxx",
+               "x...x",
+               "x...x",
+               "x...x",
+               "xxxxx"
+           );
+           var facade = new PiCrossFacade();
+           var playablePuzzle = facade.CreatePlayablePuzzle( puzzle );

+           picrossControl.Grid = playablePuzzle.Grid;
        }
    }
```

Let's run this to see what happens. You should see a 5&times;5 grid whose
squares contain some string starting with `PiCross`. The fact that there are 5&times;5
squares is a good sign. But where does that string come from?

`playablePuzzle.Grid` returns a grid, but what is its type?
Hovering over it makes a tooltip appear telling us
its type is `IGrid<IPlayablePuzzleSquare>`. `IPlayablePuzzleSquare`
is an interface; we'd prefer to know what the actual class is.
In order to find out, add a breakpoint on `MainWindow.MainWindow`'s last line.
Start the application in debug mode (F5). Hovering over `playablePuzzle.Grid`
should give you more detailed information: it's actually
a `PiCross.PlayablePuzzle.PlayablePuzzleSquare`!
This is probably what is being printed inside each square.
Let's check if we are correct about this.

Go dig into the domain and look for the `PlayablePuzzle` class. Within
it there should be a nested class `PlayablePuzzleSquare`. Extend it with a
`ToString()` method:

```diff
    private class PlayablePuzzleSquare : IPlayablePuzzleSquare
    {
        public PlayablePuzzleSquare( PlayablePuzzle parent, IVar<Square> contents, Vector2D position )
        {
            this.Contents = new PlayablePuzzleSquareContentsCell( parent, contents, position );
            this.Position = position;
        }

        Cell<Square> IPlayablePuzzleSquare.Contents => Contents;

        public PlayablePuzzleSquareContentsCell Contents { get; }

        public Vector2D Position { get; }

+       public override string ToString()
+       {
+           return "test!";
+       }
    }
```

Launch the application. Each square should now say `test!`.

During software development, it is important for you to fully comprehend what is happening.
Try to check your assumptions at each step, otherwise you might start building
things on shaky ground and sooner or later everything will collapse.
Don't let things "stay magical": the better students are those who are
willing to spend a couple of extra seconds getting a good grasp on what they are working with.

You can now remove the `ToString()` method, it serves little purpose.

