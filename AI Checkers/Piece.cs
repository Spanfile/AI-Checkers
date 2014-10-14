using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public enum PieceType
	{
		Red,
		Black
	}

	public class Piece : Transformable, Drawable
	{
		public Vector2f boardPos;
		public PieceType type;
		public bool picked;
		public bool eaten;
		public bool super;
		public Vector2f size;

		Sprite normalSprite;
		Sprite superSprite;
		Vector2f displayPos;

		public Piece(Vector2f position, Vector2f size, Color color, PieceType type)
		{
			this.size = size;
			this.boardPos = position;
			SetBoardPos(position);

			this.type = type;
			picked = false;

			normalSprite = new Sprite(new Texture("checkerspiece.png"));
			normalSprite.Origin = new Vector2f(size.X / 2, size.Y);
			normalSprite.Color = color;

			superSprite = new Sprite(new Texture("checkerspiece_double.png"));
			superSprite.Origin = new Vector2f(size.X / 2, size.Y);
			superSprite.Color = color;
		}

		public void SetBoardPos(Vector2f pos)
		{
			boardPos = pos;
			displayPos = new Vector2f(pos.X * size.X, pos.Y * size.Y) + size / 2;
		}

		public void ApplyTransform(Transform trans)
		{
			normalSprite.Position = new Vector2f(0, 0);
			superSprite.Position = new Vector2f(0, 0);

			var point = trans.TransformPoint(displayPos);
			point.Y += size.Y * 0.3f;
			point.X += size.X * 0.1f;
			Position = point;
		}

		public void SetPosition(Vector2f pos)
		{
			normalSprite.Position = pos;
			superSprite.Position = pos;
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!eaten)
			{
				if (!picked)
					states.Transform *= this.Transform;

				if (!super)
					normalSprite.Draw(target, states);
				else
					superSprite.Draw(target, states);
			}
		}
	}
}
