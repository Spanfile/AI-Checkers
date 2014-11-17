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
			uiBase = new Frame(new Vector2f(Bounds.X / 2 - 250, -400), new Vector2f(500, 300), new Color(177, 219, 222, 200));

			var font = new Font("ARIAL.TTF");
			uiBase.AddChild(new Label(new Vector2f(250, 10), "AI Checkers", font, Color.Black, 32));
			uiBase.AddChild(new Button(new Vector2f(190, 250), new Vector2f(120, 40), new Color(121, 219, 147), "Start!", font, Color.Black, 24));

			uiBaseShow = new MarqueeAnim();
			uiBaseHide = new MarqueeAnim();
		}

		public override void Load(Game game)
		{
			uiBaseShow.Start(uiBase, new Vector2f(Bounds.X / 2 - 250, Bounds.Y / 2 - 150));

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
