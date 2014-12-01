AI-Checkers
===========

A simple checkers game to which you can your make own AI.

Uses [SFML.NET](http://www.sfml-dev.org/download/sfml.net/) and .NET 4.5. woot woot.

#Gameplay basics

The games plays as 1 versus 1. Both players can be either human or AI, which means you can play player-versus-player, player-versus-computer or computer-versus-computer.

##AI

The game uses external executables as AIs. This way you can make your AI using any language you want to create your AI.

If you select either or both players as AI, you have to edit the `config.cfg` file next to the game executable.
The fields `string redAI` and `string blackAI` define the **full paths** to the AI executables. You can also edit the `string redArgs` and `string blackArgs` fields to run the AI executables with custom arguments.

###Communicating with the AI

When the game asks for the AIs turn, it will write the current board to the AIs standard input in the following format

`01010101|10101010|01010101|00000000|00000000|20202020|02020202|20202020`

where 0's represent empty squares, 1's represent squares with a red piece on it and 2's represent squares with a black piece on it.
The vertical lines are splitters there to easily see where the rows are split into lines.

After the AI has decided it's move, it has to write it into it's standard input in the following format

`mv:<piece index> <tile index>`

replacing `<piece index>` with the index of the piece it wants to move and `<tile index>` with the index of the tile it wants to move the piece on to.

Anything else the AI writes into it's standard input will be shown in the game console. This helps you debug your AI.

##Winning or losing the game

The first player to lose all their pieces loses.
If an AI is playing, the above rule is applied normally. If the AI tries to make an invalid move, such as moving a piece somewhere where it cannot be moved, moving an eaten piece or moving a piece which doesn't belong to it, the AI will instantly lose.

Stuff not implemented yet
=========================

###Super pieces
If a piece reaches the opposite end of the board (red piece reaches black's end and vice-versa), it will become a super piece. Super pieces are shown as two pieces stacked on top of each other.
Super pieces can move in any diagonal direction and have no distance limit. They can also eat multiple pieces at a time.
