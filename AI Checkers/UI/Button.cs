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

		public Button(Vector2f position, Vector2f size, Color color, string text, Font font, Color textColor, uint charSize)
			: base(position)
		{
			sprite = new Sprite(new Texture(new Image((uint)size.X, (uint)size.Y, color)));

			label = new Label(new Vector2f(size.X / 2, 0), text, font, textColor, charSize);
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
