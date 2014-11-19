using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers.UI
{
	public class Button : UIObject
	{
		Sprite sprite;
		Label label;

		Color color;

		bool msDown;

		public event EventHandler Clicked = delegate { };

		public Button(Game game, Vector2f position, Vector2f size, Color color, string text, Font font, Color textColor, uint charSize)
			: base(game, position, size)
		{
			this.color = color;

			sprite = new Sprite(new Texture(new Image((uint)size.X, (uint)size.Y, color)));

			label = new Label(game, new Vector2f(size.X / 2, size.Y / 2), text, font, textColor, charSize);
		}

		public override void Update(float frametime)
		{
			var mouse = game.GetMousePosition();

			if (Bounds.Contains(mouse.X, mouse.Y))
			{
				sprite.Color = new Color((byte)(color.R - 10), (byte)(color.G - 10), (byte)(color.B - 10), color.A);

				if (Mouse.IsButtonPressed(Mouse.Button.Left))
					msDown = true;
				else
				{
					if (msDown)
					{
						msDown = false;
						Clicked(this, EventArgs.Empty);
					}
				}
			}
			else
				sprite.Color = color;

			base.Update(frametime);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			sprite.Draw(target, states);
			label.Draw(target, states);

			base.Draw(target, states);
		}
	}
}
