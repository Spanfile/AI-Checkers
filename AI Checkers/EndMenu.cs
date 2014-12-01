using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI_Checkers.UI;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
    public class EndMenu : GameState
    {
        public override SFML.Window.Vector2f Bounds
        {
            get
            {
                return new Vector2f(1200, 675);
            }
        }

        Frame uiBase;
        MarqueeAnim uiBaseShow;
        MarqueeAnim uiBaseHide;

        PieceColor winner;

        public EndMenu()
        {

        }

        public void SetWinner(PieceColor winner)
        {
            this.winner = winner;
        }

        public override void Load(Game game)
        {
            var font = new Font("ARIAL.TTF");

            uiBaseShow = new MarqueeAnim();
            uiBaseHide = new MarqueeAnim();

            uiBase = new Frame(game, new Vector2f(Bounds.X / 2 - 250, -400), new Vector2f(500, 300), new Color(177, 219, 222, 200));

            var restartButton = new Button(game, new Vector2f(190, 200), new Vector2f(120, 40), new Color(121, 219, 147), "Restart", font, Color.Black, 24);
            var menuButton = new Button(game, new Vector2f(190, 250), new Vector2f(120, 40), new Color(121, 219, 147), "Main menu", font, Color.Black, 24);

            restartButton.Clicked += (s, e) => uiBaseHide.Start(uiBase, new Vector2f(Bounds.X / 2 - 250, Bounds.Y + 100), 0.2f);

            uiBase.AddChild(new Label(game, new Vector2f(250, 20), String.Format("{0} has won the game!", winner), font, winner == PieceColor.Red ? Color.Red : Color.Black, 32));

            uiBase.AddChild(restartButton);
            uiBase.AddChild(menuButton);

            uiBaseHide.Stopped += (s, e) =>
            {
                GameState.Ingame.Start();
                game.SetActiveGameState(GameState.Ingame);
            };

            base.Load(game);
        }

        public void Show()
        {
            uiBase.Position = new Vector2f(Bounds.X / 2 - 250, -400);
            uiBaseShow.Start(uiBase, new Vector2f(Bounds.X / 2 - 250, Bounds.Y / 2 - 150), 0.2f);
        }

        public override void Update(float frametime)
        {
            uiBase.Update(frametime);
            uiBaseShow.Update(frametime);
            uiBaseHide.Update(frametime);

            base.Update(frametime);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            GameState.Ingame.DrawBoardDecor(target, states);

            uiBase.Draw(target, states);

            base.Draw(target, states);
        }
    }
}
