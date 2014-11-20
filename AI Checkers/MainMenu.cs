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
    public class MainMenu : GameState
    {
        public override Vector2f Bounds
        {
            get
            {
                return new Vector2f(1200, 675);
            }
        }

        Frame uiBase;
        MarqueeAnim uiBaseShow;
        MarqueeAnim uiBaseHide;

        public MainMenu()
        {

        }

        public override void Load(Game game)
        {
            var font = new Font("ARIAL.TTF");

            uiBaseShow = new MarqueeAnim();
            uiBaseHide = new MarqueeAnim();

            uiBase = new Frame(game, new Vector2f(Bounds.X / 2 - 250, -400), new Vector2f(500, 300), new Color(177, 219, 222, 200));

            var startButton = new Button(game, new Vector2f(190, 250), new Vector2f(120, 40), new Color(121, 219, 147), "Start!", font, Color.Black, 24);
            startButton.Clicked += (s, e) => uiBaseHide.Start(uiBase, new Vector2f(Bounds.X / 2 - 250, Bounds.Y + 100), 0.2f);

            var ply1Check = new Checkbox(game, new Vector2f(10, 80), "Player 1 AI", Color.Black, font);
            var ply2Check = new Checkbox(game, new Vector2f(10, 130), "Player 2 AI", Color.Black, font);

            var ply1ai = false;
            var ply2ai = false;

            ply1Check.CheckChanged += (s, e) => ply1ai = e.Checked;
            ply2Check.CheckChanged += (s, e) => ply2ai = e.Checked;

            uiBase.AddChild(new Label(game, new Vector2f(250, 20), "AI Checkers", font, Color.Black, 32));
            uiBase.AddChild(startButton);
            uiBase.AddChild(ply1Check);
            uiBase.AddChild(ply2Check);

            uiBaseHide.Stopped += (s, e) =>
            {
                GameState.Ingame.SetPlayerAI(ply1ai, ply2ai);
                GameState.Ingame.Start();
                game.SetActiveGameState(GameState.Ingame);
            };

            uiBaseShow.Start(uiBase, new Vector2f(Bounds.X / 2 - 250, Bounds.Y / 2 - 150), 0.2f);

            GameState.Ingame.Load(game);

            base.Load(game);
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
            GameState.Ingame.DrawBoard(target, states);

            uiBase.Draw(target, states);

            base.Draw(target, states);
        }
    }
}
