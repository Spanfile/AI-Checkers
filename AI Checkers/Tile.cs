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
		Vertex[] verts;

		public Tile(Vector2f position, Vector2f size)
		{
			this.Position = new Vector2f(position.X * size.X, position.Y * size.Y);

			var light = new Color(204, 150, 73);
			var dark = new Color(148, 93, 16);

			var color = dark;

			if (position.X % 2 == position.Y % 2) // woot woot. makes that chessboard pattern
				color = light;

			verts = new Vertex[]
			{
				new Vertex(new Vector2f(0, 0), color),
				new Vertex(new Vector2f(size.X, 0), color),
				new Vertex(new Vector2f(size.X, size.Y), color),
				new Vertex(new Vector2f(0, size.Y), color)
			};
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= this.Transform;
			target.Draw(verts, PrimitiveType.Quads, states);
		}
	}
}
