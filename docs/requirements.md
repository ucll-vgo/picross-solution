# Functionality

This page describes the minimal functionality you should implement.

**NOTE** This page is work in progress.

## Minimum

* Playable PiCross puzzles
  * Visualization of grid and constraints
  * Possibility to mark squares as filled, empty or unknown
  * Visual distinction between satisfied and unsatisfied constraints (on both levels: entire constraints and single numbers)
  * Notification when puzzle is solved
* Multiple Screens (these are not necessarily multiple windows! The same GUI window can be reused, which implementation-wise is easier to achieve)
  * Introduction Screen
  * Puzzle Solving Screen
  * Puzzle Selection Screen
* Tolerable aesthetics
  * Centered text where appropriate
  * Some brushes to liven up
  * We will not question your aesthetic choices, but we require you to at least make some effort and use brushes/fonts/...
* Make commands/converters/... as reusable as possible
* MVVM-compliant

## Extensions

* Puzzle Solving
  * Help button: when clicked, all erroneously marked cells get reset to unknown.
  * Timer
  * Allow player to select a range (horizontal/vertical) of squares to mark them all in one step.
  * Thumbnail view (see `PiCrossControl`)
* Support for multiple players (not in parallel, just a login/logout-like system)
  * Log in as player (requires extra screen showing all players)
  * Log out (return to player selection screen)
  * Add new player
  * Remove player
* Puzzle Selection
  * Show thumbnail if already solved
  * Group in solved/nonsolved, by size
  * Best times
  * Filter to show only unsolved puzzles
  * Filter tho show only puzzles of specific size
* Puzzle Editor
  * Check for ambiguity (functionality readily present in domain code)
* Sound/Music
* Animations
* Themes

## Notes

You are allowed to extend/modify the given code, but
respect the overall structure of the code, e.g., don't
add view-specific code in the domain.
You need to be able to defend every change you make.
