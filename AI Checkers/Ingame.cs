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
				return new Vector2f(1200, 675); // 1200, 675
			}
		}

		List<Tile> tiles;
		List<Piece> pieces;

		Transform boardTransform;
		Vector2f boardSize;

		Color light = new Color(204, 150, 73);
		Color dark = new Color(148, 93, 16);

		public Ingame(Game game)
			: base(game)
		{
		}

		public override void Load()
		{
			tiles = new List<Tile>();
			pieces = new List<Piece>();

			boardSize = new Vector2f(Math.Min(Bounds.X, Bounds.Y) - 50, Math.Min(Bounds.X, Bounds.Y) - 50);

			var tileSize = new Vector2f(boardSize.X / 8, boardSize.Y / 8);
			for (var y = 0; y < 8; y++)
				for (var x = 0; x < 8; x++)
				{
					if (x % 2 == y % 2)
					{
						tiles.Add(new Tile(new Vector2f(x, y), tileSize, dark));

						if (y < 3)
							pieces.Add(new Piece(new Vector2f(x, y), tileSize, dark));
						else if (y > 4)
							pieces.Add(new Piece(new Vector2f(x, y), tileSize, light));
					}
					else
						tiles.Add(new Tile(new Vector2f(x, y), tileSize, light));
				}

			var center = new Vector2f(Bounds.X / 2f, Bounds.Y / 2f);

			boardTransform = Transform.Identity;
			boardTransform.Translate(center - new Vector2f(boardSize.X / 2f, boardSize.Y / 2f));

			var scale = Transform.Identity;
			scale.Scale(new Vector2f(1.25f, 0.5f), new Vector2f(boardSize.X / 2, boardSize.Y / 2));
			boardTransform.Combine(scale);

			base.Load();
		}

		public override void Update(float frametime)
		{
			var center = new Vector2f(boardSize.X / 2, boardSize.Y / 2);

			if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
				boardTransform.Rotate(-0.5f, center);
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
				boardTransform.Rotate(0.5f, center);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= boardTransform;

			foreach (var tile in tiles)
				tile.Draw(target, states);

			foreach (var piece in pieces)
			{
				piece.TransformPosition(boardTransform);
				target.Draw(piece);
			}
		}
	}
}
