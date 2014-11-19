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

		int pickedIndex;
		PieceType turn;

		bool ply1ai;
		bool ply2ai;

		public Ingame()
		{
			pickedIndex = -1;

			turn = PieceType.Red;
		}

		public void SetPlayerAI(bool ply1, bool ply2)
		{
			ply1ai = ply1;
			ply2ai = ply2;
		}

		void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			if (!Active)
				return;

			var mouse = game.GetMousePosition();

			if (e.Button == Mouse.Button.Left)
			{
				#region Piece moving
				var selected = (from t
								in tiles
								where t.ContainsPoint(new Vector2f(mouse.X, mouse.Y), boardTransform)
								select t).ToList();
				if (!selected.Any())
					return;

				var tile = selected.First();

				Console.WriteLine("Tile: {0}", tile.boardPos);

				if (pickedIndex == -1)
				{
					var pieces = this.pieces.Where(
						p => p.boardPos.X == tile.boardPos.X &&
						p.boardPos.Y == tile.boardPos.Y &&
						!p.eaten).ToList();

					if (!pieces.Any())
						return;

					var piece = pieces.First();

					if (piece.type != turn)
						return;

					piece.picked = true;
					pickedIndex = this.pieces.IndexOf(piece);

					Console.WriteLine("{0} piece picked up", piece.type.ToString());
				}
				else
				{
					var allowed = GetPossibleMoveIndices(pickedIndex);

					if (!allowed.Contains(tiles.IndexOf(tile)))
						return;

					var moved = !pieces[pickedIndex].boardPos.Equals(tile.boardPos);
					var previous = pieces[pickedIndex].boardPos;

					if (!moved)
						Console.WriteLine("Piece position not changed");

					pieces[pickedIndex].SetBoardPos(tile.boardPos);
					pieces[pickedIndex].picked = false;

					Console.WriteLine("{0} piece dropped at {1}", pieces[pickedIndex].type.ToString(), tile.boardPos);

					if (pieces[pickedIndex].type == PieceType.Red)
					{
						if (tile.boardPos.Y == 7)
						{
							pieces[pickedIndex].super = true;
							Console.WriteLine("Holy shit the piece has become a super piece");
						}
					}
					else
					{
						if (tile.boardPos.Y == 0)
						{
							pieces[pickedIndex].super = true;
							Console.WriteLine("Holy shit the piece has become a super piece");
						}
					}

					var pieceEaten = false;
					// check if a piece was eaten
					if (moved)
					{
						// okay so we moved, how much?
						var change = tile.boardPos - previous;
						//Console.WriteLine("Piece position changed by {0}", change);
						if (Math.Abs(change.X) > 1 && Math.Abs(change.Y) > 1)
						{
							// we may have jumped over a piece

							var gcd = (float)Math.Abs(GreatestCommonDivisor(change.X, change.Y));
							var direction = new Vector2f(change.X / gcd, change.Y / gcd);
							//Console.WriteLine("Change: {0}, GCD: {1}, dir: {2}", change, gcd, direction);

							var inverse = pieces[pickedIndex].type == PieceType.Red ? PieceType.Black : PieceType.Red;

							// traverse backwards, eating any pieces that we find
							var point = tile.boardPos - direction;
							while (true)
							{
								if (IsPieceAt(point, inverse))
								{
									Console.WriteLine("{0} piece eaten at {1}", pieces[GetIndexOfPiece(point)].type, point);
									pieces[GetIndexOfPiece(point)].eaten = true;
									pieceEaten = true;
								}

								point -= direction;
								if (point.Equals(previous))
									break;
							}
						}
					}

					HandleTurn(moved, pieceEaten);

					pickedIndex = -1;
				}
				#endregion
			}
			else if (e.Button == Mouse.Button.Right)
			{
				var selected = (from t
								in tiles
								where t.ContainsPoint(new Vector2f(mouse.X, mouse.Y), boardTransform)
								select t).ToList();
				if (!selected.Any())
					return;

				var tile = selected.First();

				Console.WriteLine("Tile: {0}", tile.boardPos);

				var pieces = this.pieces.Where(
					p => p.boardPos.X == tile.boardPos.X &&
					p.boardPos.Y == tile.boardPos.Y &&
					!p.eaten).ToList();

				if (!pieces.Any())
					return;

				var piece = pieces.First();

				piece.super = !piece.super;
			}
		}

		void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
			if (!Active)
				return;
		}

		void HandleTurn(bool moved, bool pieceEaten)
		{
			if (!moved)
				return;

			if (!pieceEaten)
				turn = turn == PieceType.Red ? PieceType.Black : PieceType.Red;

			Console.WriteLine("It is now {0}'s turn", turn.ToString());
		}

		int[] GetPossibleMoveIndices(int pieceIndex)
		{
			var piece = pieces[pieceIndex];
			var y = piece.type == PieceType.Red ? -1 : 1; // red = -1, black = 1
			var inverse = piece.type == PieceType.Red ? PieceType.Black : PieceType.Red;

			List<int> indices = new List<int>();

			indices.Add(GetIndexOfTile(piece.boardPos));

			if (!piece.super)
			{
				CheckPointRecursion(piece.boardPos, new Vector2f(-1, -y), piece.type, piece.super, ref indices);
				CheckPointRecursion(piece.boardPos, new Vector2f(1, -y), piece.type, piece.super, ref indices);
			}
			else
			{
				CheckPointRecursion(piece.boardPos, new Vector2f(-1, y), piece.type, piece.super, ref indices);
				CheckPointRecursion(piece.boardPos, new Vector2f(1, y), piece.type, piece.super, ref indices);
				CheckPointRecursion(piece.boardPos, new Vector2f(-1, -y), piece.type, piece.super, ref indices);
				CheckPointRecursion(piece.boardPos, new Vector2f(1, -y), piece.type, piece.super, ref indices);
			}

			return indices.ToArray();
		}

		void CheckPointRecursion(Vector2f point, Vector2f direction, PieceType type, bool isSuper, ref List<int> indices)
		{
			var check = point + direction;
			var inverse = type == PieceType.Red ? PieceType.Black : PieceType.Red;

			if (check.X < 0 || check.X > 7 || check.Y < 0 || check.Y > 7)
				return;

			if (!IsPieceAt(check))
			{
				indices.Add(GetIndexOfTile(check));
				if (isSuper)
					CheckPointRecursion(check, direction, type, isSuper, ref indices);
			}
			else if (IsPieceAt(check, inverse))
			{
				if (!isSuper)
				{
					if (!IsPieceAt(check + direction))
						indices.Add(GetIndexOfTile(check + direction));
				}
				else
				{
					while (true)
					{
						if (!IsPieceAt(check + direction))
						{
							indices.Add(GetIndexOfTile(check + direction));
							break;
						}

						check += direction;
						if (check.X < 0 || check.X > 7 || check.Y < 0 || check.Y > 7)
							break;
					}
				}
			}
		}

		bool IsPieceAt(Vector2f point)
		{
			return (from piece in pieces
					where piece.boardPos.Equals(point) && !piece.eaten
					select piece).Any();
		}

		bool IsPieceAt(Vector2f point, PieceType type)
		{
			return (from piece in pieces
					where piece.boardPos.Equals(point) && piece.type == type && !piece.eaten
					select piece).Any();
		}

		int GetIndexOfPiece(Vector2f point)
		{
			if (!IsPieceAt(point))
				return -1;

			return pieces.IndexOf((from piece in pieces
								   where piece.boardPos.Equals(point) && !piece.eaten
								   select piece).First());
		}

		int GetIndexOfTile(Vector2f point)
		{
			if (point.X < 0 || point.X > 7 || point.Y < 0 || point.Y > 7)
				return -1;

			return tiles.IndexOf((from tile in tiles
								  where tile.boardPos.Equals(point)
								  select tile).First());
		}

		int GetPieceCount(PieceType type)
		{
			return (from piece in pieces
					where piece.type == type
					select piece).Count();
		}

		double GreatestCommonDivisor(double a, double b)
		{
			if (b == 0)
				return a;

			return GreatestCommonDivisor(b, a % b);
		}

		public override void Load(Game game)
		{
			game.Window.MouseButtonPressed += Window_MouseButtonPressed;
			game.Window.MouseButtonReleased += Window_MouseButtonReleased;

			tiles = new List<Tile>();
			pieces = new List<Piece>();

			boardSize = new Vector2f(Math.Min(Bounds.X, Bounds.Y) - 50, Math.Min(Bounds.X, Bounds.Y) - 50);

			var tileSize = new Vector2f(boardSize.X / 8, boardSize.Y / 8);
			for (var y = 0; y < 8; y++)
				for (var x = 0; x < 8; x++)
				{
					if (x % 2 != y % 2)
					{
						tiles.Add(new Tile(new Vector2f(x, y), tileSize, dark));

						if (y < 3)
							pieces.Add(new Piece(new Vector2f(x, y), tileSize, new Color(247, 39, 39), PieceType.Red));
						else if (y > 4)
							pieces.Add(new Piece(new Vector2f(x, y), tileSize, new Color(66, 66, 66), PieceType.Black));
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

			boardTransform.Rotate(45f, new Vector2f(boardSize.X / 2f, boardSize.Y / 2f));

			base.Load(game);
		}

		public override void Update(float frametime)
		{

		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			var mouse = game.GetMousePosition();

			states.Transform *= boardTransform;

			var moveIndices = new int[] { };
			if (pickedIndex != -1)
				moveIndices = GetPossibleMoveIndices(pickedIndex);

			foreach (var tile in tiles)
			{
				tile.Draw(target, states);

				var point = tile.ContainsPoint(new Vector2f(mouse.X, mouse.Y), boardTransform);
				var move = moveIndices.Contains(tiles.IndexOf(tile));

				if (point || move)
				{
					var color = point ? Color.Red : Color.Yellow;

					var verts = (from vert in tile.GetVerts()
								 select new Vertex(vert.Position + tile.Position, color)).ToList();
					verts.Add(verts.First());
					target.Draw(verts.ToArray(), PrimitiveType.LinesStrip, states);
				}
			}

			foreach (var piece in pieces)
			{
				if (pieces.IndexOf(piece) != pickedIndex)
					piece.ApplyTransform(boardTransform);

				target.Draw(piece);
			}

			if (pickedIndex != -1)
			{
				pieces[pickedIndex].SetPosition(new Vector2f(mouse.X + 8f, mouse.Y + 15f));
				target.Draw(pieces[pickedIndex]);
			}
		}

		/// <summary>
		/// Used to draw the board only as a decoration
		/// </summary>
		/// <param name="target"></param>
		/// <param name="states"></param>
		public void DrawBoard(RenderTarget target, RenderStates states)
		{
			states.Transform *= boardTransform;

			foreach (var tile in tiles)
				tile.Draw(target, states);

			foreach (var piece in pieces)
			{
				piece.ApplyTransform(boardTransform);
				target.Draw(piece);
			}
		}
	}
}
