AI-Checkers
===========

A simple checkers game to which you can your make own AI.

Uses [SFML.NET](http://www.sfml-dev.org/download/sfml.net/) and .NET 4.5. woot woot.

#Gameplay basics

The games plays as 1 versus 1. Both players can be either human or AI, which means you can play player-versus-player, player-versus-computer or computer-versus-computer.

##AI

The game uses external executables as AIs. This way you can make your AI using any language you want to create your AI.

If you select either or both players as AI, you have to edit the `config.cfg` file next to the game executable.
The fields `string redAI` and `string blackAI` define the **full paths** to the AI executables. You can also edit the `string redArgs` and `string blackArgs` fields to run the AI executables with custom arguments. More on the config file down there.

###Communicating with the AI

When the game asks for the AIs turn, it will write the current board to the AIs standard input in the following format

`00/10/00/10/00/10/00/10/|10/00/10/00/10/00/10/00/|00/10/00/10/00/10/00/10/|00/00/00/00/00/00/00/00/|00/00/00/00/00/00/00/00/|20/00/20/00/20/00/20/00/|00/20/00/20/00/20/00/20/|20/00/20/00/20/00/20/00/`

That may look complicated but is actually quite simple. Each row of the board is split with a vertical line `|`. Each tile is split with a slash `/`. The tiles are represented with two numbers. The first number tells whether the tile has a piece on it and which color the piece is. `0` means no piece, `1` means red piece and `2` means black piece. The other number tells whether the piece on it is a super piece. `0` means normal piece and `1` means super piece.

After the AI has decided it's move, it has to write it into it's standard input in the following format

`mv:<piece index> <tile index>`

replacing `<piece index>` with the index of the piece it wants to move and `<tile index>` with the index of the tile it wants to move the piece on to.

Once the game ends, the AI will be closed. The game will write "exit" to it's standard input and after the AI receives the command, it should close itself.

Anything else the AI writes into it's standard input will be shown in the game console. This helps you debug your AI.

##Winning or losing the game

The first player to lose all their pieces loses.
If an AI is playing, the above rule is applied normally. If the AI tries to make an invalid move, such as moving a piece somewhere where it cannot be moved, moving an eaten piece or moving a piece which doesn't belong to it, the AI will instantly lose.

##Config

Next to the game executable is a config file, `config.cfg`. In it, you'll find settings for the game. The AI executable and argument fields have been explained above.

- `useLineSeparator` defines whether the map format contains the row splitting vertical lines.

- `usePieceSeparator` defines whether the map format contains the piece splitting slashes.

- `killAI` defines should the AI executable be told to close or just forcibly killed when the game ends.

