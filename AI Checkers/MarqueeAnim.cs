using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public class MarqueeAnim : Animation
	{
		Vector2f to;

		public MarqueeAnim()
		{

		}

		public void Start(Transformable target, Vector2f to)
		{
			this.to = to;
			base.Start(target);
		}

		protected override void InternalUpdate(float frametime, Transformable target)
		{
			var dist = target.Position.Distance(to);

			if (dist < 1)
			{
				Stop();
			}

			var dir = (to - target.Position).Normalize();

			var pos = dir * dist / 10;
			target.Position += new Vector2f((int)pos.X, (int)pos.Y);
		}
	}
}
