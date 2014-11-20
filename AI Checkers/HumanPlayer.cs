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

        ManualResetEvent doneEvent;

        Ingame ingame;

        public HumanPlayer(Game game, PieceColor color)
            : base(game, color)
        {
            ingame = GameState.Ingame;

            doneEvent = new ManualResetEvent(false);
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

                if (!allowed.Contains(ingame.tiles.IndexOf(clicked)))
                    return;

                ingame.pieces[picked].picked = false;

                to = ingame.tiles.IndexOf(clicked);

                doneEvent.Set();

                Console.WriteLine("{0}: Piece dropped", PlayerColor);
            }
        }

        public async override Task<Tuple<int, int>> GetMove()
        {
            active = true;

            return await HandleMove();
        }

        Task<Tuple<int, int>> HandleMove()
        {
            return Task.Run<Tuple<int, int>>(() =>
            {
                doneEvent.WaitOne();
                doneEvent.Reset();

                active = false;

                var result = new Tuple<int, int>(picked, to);

                picked = -1;
                ingame.pickedIndex = -1;
                to = -1;

                return result;
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
                    Console.WriteLine("{0}: Click!", PlayerColor);

                    msDown = false;

                    var selected = ingame.tiles.Find((t) => t.ContainsPoint(mouse, GameState.Ingame.boardTransform));
                    Clicked(selected);
                }
            }
        }
    }
}
