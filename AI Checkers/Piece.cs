using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public class Piece : Transformable, Drawable
	{
		Sprite sprite;
		Vector2f size;
		Vector2f boardPos;

		public Piece(Vector2f position, Vector2f size, Color color)
		{
			this.size = size;
			boardPos = new Vector2f(position.X * size.X, position.Y * size.Y) + size / 2;

			sprite = new Sprite(new Texture("checkerspiece.png"));
			sprite.Origin = new Vector2f(size.X / 2, size.Y);
			sprite.Color = color;
		}

		public void TransformPosition(Transform trans)
		{
			var point = trans.TransformPoint(boardPos);
			point.Y += size.Y * 0.3f;
			Position = point;
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= this.Transform;
			sprite.Draw(target, states);
		}
	}
}
