using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public class Tile : Transformable, Drawable
	{
		public Vector2f boardPos;
		Vertex[] verts;

		Text up;
		Text down;
		Text left;
		Text right;

		char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
		char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8' };

		public Tile(Vector2f position, Vector2f size, Color color)
		{
			this.boardPos = position;
			this.Position = new Vector2f(position.X * size.X, position.Y * size.Y);

			verts = new Vertex[]
			{
				new Vertex(new Vector2f(0, 0), color),
				new Vertex(new Vector2f(size.X, 0), color),
				new Vertex(new Vector2f(size.X, size.Y), color),
				new Vertex(new Vector2f(0, size.Y), color)
			};

			var font = new Font("ARIAL.TTF");
			up = new Text("", font, 18);
			down = new Text(up);
			left = new Text(up);
			right = new Text(up);
			if (position.X == 0)
			{
				left.Position = new Vector2f(boardPos.X - 20f, boardPos.Y + (size.Y / 2f) - 8f);
				left.DisplayedString = numbers[(int)position.Y].ToString();
			}
			else if (position.X == 7)
			{
				right.Position = new Vector2f(boardPos.X + size.X, boardPos.Y + (size.Y / 2f) - 8f);
				right.DisplayedString = numbers[(int)position.Y].ToString();
			}

			if (position.Y == 0)
			{
				up.Position = new Vector2f(boardPos.X + (size.X / 2f) - 8f, boardPos.Y - 24f);
				up.DisplayedString = letters[(int)position.X].ToString();
			}

			if (position.Y == 7)
			{
				down.Position = new Vector2f(boardPos.X + (size.X / 2f) - 8f, boardPos.Y + size.Y);
				down.DisplayedString = letters[(int)position.X].ToString();
			}
		}

		public Vertex[] GetVerts()
		{
			return verts;
		}

		public bool ContainsPoint(Vector2f point, Transform trans)
		{
			var contains = false;

			// http://dominoc925.blogspot.fi/2012/02/c-code-snippet-to-determine-if-point-is.html
			for (int i = 0, j = verts.Length - 1; i < verts.Length; j = i++)
			{
				var pointI = trans.TransformPoint(verts[i].Position + Position);
				var pointJ = trans.TransformPoint(verts[j].Position + Position);

				if (((pointI.Y > point.Y) != (pointJ.Y > point.Y)) &&
					(point.X < (pointJ.X - pointI.X) * (point.Y - pointI.Y) /
					(pointJ.Y - pointI.Y) + pointI.X))
					contains = !contains;
			}

			return contains;
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= this.Transform;
			target.Draw(verts, PrimitiveType.Quads, states);

			up.Draw(target, states);
			down.Draw(target, states);
			left.Draw(target, states);
			right.Draw(target, states);
		}
	}
}
