using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers.UI
{
	public class Frame : UIObject
	{
		Sprite sprite;

		public Frame(Vector2f position, Vector2f size, Color color)
			: base(position)
		{
			sprite = new Sprite(new Texture(new Image((uint)size.X, (uint)size.Y, color)));
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			sprite.Draw(target, states);

			base.Draw(target, states);
		}
	}
}
