using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
    public class HumanPlayer : Player
    {
        bool active = false;
        bool msDown = false;
        
        int picked = -1;
        int to = -1;

        Tuple<int, int> result;

        Ingame ingame;

        public HumanPlayer(Game game, PieceColor color)
            : base(game, color)
        {
            ingame = GameState.Ingame;
        }

        void Clicked(Tile clicked)
        {
            if (clicked == null)
                return;

            Piece piece;

            if (picked == -1)
            {
                piece = ingame.pieces.Find((p) => p.boardPos.Equals(clicked.boardPos) && !p.eaten && p.color == PlayerColor);

                if (piece != null)
                {
                    piece.picked = true;
                    picked = ingame.pieces.IndexOf(piece);
                    ingame.pickedIndex = picked;

                    Console.WriteLine("{0}: Piece picked up: {1}", PlayerColor, picked);
                }
            }
            else
            {
                var allowed = ingame.GetPossibleMoveIndices(picked);
                piece = ingame.pieces[picked];

                if (!allowed.Contains(ingame.tiles.IndexOf(clicked)))
                    return;

                piece.picked = false;
                ingame.pickedIndex = -1;

                if (!piece.boardPos.Equals(clicked.boardPos))
                {
                    to = ingame.tiles.IndexOf(clicked);
                    result = new Tuple<int, int>(picked, to);

                    state = PlayerState.Finished;
                }

                picked = -1;

                Console.WriteLine("{0}: Piece dropped", PlayerColor);
            }
        }

        public override void StartMove()
        {
            active = true;

            result = null;
            picked = -1;
            to = -1;

            base.StartMove();
        }

        public override Tuple<int, int> GetMove()
        {
            if (result != null)
                return result;

            Console.WriteLine("{0}: GetMove called, result is null", PlayerColor);
            return new Tuple<int, int>(-1, -1);
        }

        public override void Update(float frametime)
        {
            if (!active)
                return;

            var mouse = new Vector2f(game.GetMousePosition().X, game.GetMousePosition().Y);

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
                msDown = true;
            else
            {
                if (msDown == true)
                {
                    //Console.WriteLine("{0}: Click!", PlayerColor);

                    msDown = false;

                    var selected = ingame.tiles.Find((t) => t.ContainsPoint(mouse, GameState.Ingame.boardTransform));
                    Clicked(selected);
                }
            }
        }
    }
}
