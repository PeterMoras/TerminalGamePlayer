# TerminalGamePlayer
A simple project for testing IoC with DI

The purpose of this project is to test using the Inversion of Control principle, with Dependency Injection as the method of achieving this.
The code itself is a bit messy, but the classes themselves are well seperated by concerns and classes don't need to worry about their dependencies, making iterations for this project quick.

## Functionality
Starting up the project clears the console and displays a game select with options.
Currently only Connect4 exists, but games can easily be coded in by adding a class to DI that implements IGame and adding to the GameList enum.

The Connect4 is the connect 4 game with an AI that places pieces in random rows.


## Future Progress
Adding other games in the future if I feel like it.
Adding smart/optimal AIs for games.


## Bugs
1. Classes created in the Game Scope are not all removed when the game completes. Ideally when the scope is disposed, all classes in the scope are also disposed of, even if they have running Tasks.
This has been solved by making sure classes with background work properly implement IDisposable to prevent data leaks.

2. Poor Rule design for Connect4 allows the AI to take a turn after the player already wins. This should be fixed by creating a specific Connect4 turn/rule manager to better handle edges cases like this.
