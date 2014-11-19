using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public static class Extensions
	{
		public static float Length(this Vector2f vec)
		{
			return (float)Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
		}

		public static float Distance(this Vector2f from, Vector2f to)
		{
			return Length(to - from);
		}

		public static Vector2f Normalize(this Vector2f vec)
		{
			var length = vec.Length();
			return new Vector2f(vec.X / length, vec.Y / length);
		}

		public static float Dot(this Vector2f from, Vector2f to)
		{
			return from.X * to.X + from.Y * to.Y;
		}

		public static Vector2f Lerp(this Vector2f from, Vector2f to, float fraction)
		{
			return new Vector2f(from.X + (to.X - from.X) * fraction, from.Y + (to.Y - from.Y) * fraction);
		}
	}
}
