using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers.UI
{
	public class CheckboxEventArgs : EventArgs
	{
		public bool Checked { get; private set; }

		public CheckboxEventArgs(bool check)
		{
			Checked = check;
		}
	}

	public class Checkbox : UIObject
	{
		public bool Checked { get; private set; }

		Label label;
		Sprite box;

		Color uncheck;
		Color check;

		bool msDown;

		public event EventHandler<CheckboxEventArgs> CheckChanged = delegate { };

		public Checkbox(Game game, Vector2f position, string text, Color textColor, Font font)
			: base(game, position, new Vector2f(32, 32))
		{
			uncheck = new Color(191, 191, 191);
			check = new Color(121, 219, 147);

			label = new Label(game, new Vector2f((int)90, (int)8), text, font, textColor, 20);

			box = new Sprite(new Texture(new Image(32, 32, new Color(191, 191, 191))));
		}

		public override void Update(float frametime)
		{
			var mouse = game.GetMousePosition();

			if (Bounds.Contains(mouse.X, mouse.Y))
			{
				if (Checked)
					box.Color = new Color((byte)(check.R - 10), (byte)(check.G - 10), (byte)(check.B - 10), check.A);
				else
					box.Color = new Color((byte)(uncheck.R - 10), (byte)(uncheck.G - 10), (byte)(uncheck.B - 10), uncheck.A);

				if (Mouse.IsButtonPressed(Mouse.Button.Left))
					msDown = true;
				else
				{
					if (msDown)
					{
						msDown = false;
						Checked = !Checked;
						CheckChanged(this, new CheckboxEventArgs(Checked));
					}
				}
			}
			else
			{
				if (Checked)
					box.Color = check;
				else
					box.Color = uncheck;

			}

			base.Update(frametime);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;

			box.Draw(target, states);
			label.Draw(target, states);

			base.Draw(target, states);
		}
	}
}
