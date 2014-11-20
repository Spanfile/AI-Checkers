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
		float initialDist;
		float time;
		float speed;

		public MarqueeAnim()
		{

		}

		public void Start(Transformable target, Vector2f to, float speed)
		{
			this.to = to;
			this.speed = speed;

			initialDist = target.Position.Distance(to);
			time = 0;

			base.Start(target);
		}

		protected override void InternalUpdate(float frametime, Transformable target)
		{
			time += frametime;

			var dist = target.Position.Distance(to);
			var travelled = time * speed;

			target.Position = target.Position.Lerp(to, travelled / initialDist);
			//Console.WriteLine(dist);
			if (dist < 1)
			{
				target.Position = new Vector2f((int)to.X, (int)to.Y);
				//Console.WriteLine("Stopped");
				Stop();
			}
		}
	}
}
