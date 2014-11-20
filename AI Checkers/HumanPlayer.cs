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

        AutoResetEvent doneEvent;

        Ingame ingame;

        public HumanPlayer(Game game, PieceType color)
            : base(game, color)
        {
            ingame = GameState.Ingame;

            doneEvent = new AutoResetEvent(false);
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

                    Console.WriteLine("Piece picked up: {0}", picked);
                }
            }
            else
            {
                var allowed = ingame.GetPossibleMoveIndices(picked);

                if (!allowed.Contains(ingame.tiles.IndexOf(clicked)))
                    return;

                piece = ingame.pieces[picked];

                piece.picked = false;
                picked = -1;
                ingame.pickedIndex = -1;

                to = ingame.tiles.IndexOf(clicked);

                doneEvent.Set();

                Console.WriteLine("Piece dropped");
            }
        }

        public async override Task<Tuple<int, int>> GetMove()
        {
            active = true;

            await HandleMove();

            return result;
        }

        Task HandleMove()
        {
            return new Task(() =>
            {
                doneEvent.WaitOne();

                result = new Tuple<int, int>(picked, to);
                active = false;
            });
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
                    Console.WriteLine("Click!");

                    msDown = false;

                    var selected = ingame.tiles.Find((t) => t.ContainsPoint(mouse, GameState.Ingame.boardTransform));
                    Clicked(selected);
                }
            }
        }
    }
}
