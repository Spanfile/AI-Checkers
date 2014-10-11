using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public class Ingame : GameState
	{
		public override Vector2f Bounds
		{
			get
			{
				return new Vector2f(1350, 675);
			}
		}

		List<Tile> tiles;
		RenderTexture renderTex;

		public Ingame()
		{
		}

		public override void Load()
		{
			tiles = new List<Tile>();

			var size = new Vector2f(Bounds.X / 8, Bounds.Y / 8);
			for (var y = 0; y < 8; y++)
				for (var x = 0; x < 8; x++)
					tiles.Add(new Tile(new Vector2f(x, y), size));

			renderTex = new RenderTexture((uint)Bounds.X, (uint)Bounds.Y);

			base.Load();
		}

		public override void Update(float frametime)
		{
			
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			renderTex.Clear(Color.Transparent);

			foreach (var tile in tiles)
				tile.Draw(renderTex, states);

			renderTex.Display();

			var verts = new Vertex[]
			{
				new Vertex(new Vector2f(Bounds.X / 2f, 0), new Vector2f(0, 0)),
				new Vertex(new Vector2f(Bounds.X, Bounds.Y / 2f), new Vector2f(renderTex.Size.X, 0)),
				new Vertex(new Vector2f(Bounds.X / 2f, Bounds.Y), new Vector2f(renderTex.Size.X, renderTex.Size.Y)),
				new Vertex(new Vector2f(0, Bounds.Y / 2f), new Vector2f(0, renderTex.Size.Y))
			};

			states.Texture = renderTex.Texture;

			//var center = new Vector2f(target.Size.X / 2f, target.Size.Y / 2f);
			//states.Transform.Scale(new Vector2f(1, 0.5f), center);

			target.Draw(verts, PrimitiveType.Quads, states);
		}
	}
}
