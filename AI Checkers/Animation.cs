using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public enum AnimState
	{
		Stopped,
		Running,
	}

	public abstract class Animation
	{
		AnimState state;
		Transformable target;

		public event EventHandler Started = delegate { };
		public event EventHandler Stopped = delegate { };

		public Animation()
		{
			state = AnimState.Stopped;
		}

		public void Start(Transformable target)
		{
			state = AnimState.Running;
			this.target = target;

			Started(this, EventArgs.Empty);
		}

		public void Stop()
		{
			state = AnimState.Stopped;
			target = null;

			Stopped(this, EventArgs.Empty);
		}

		public void Update(float frametime)
		{
			if (state == AnimState.Running)
				InternalUpdate(frametime, target);
		}

		/// <summary>
		/// Used as overridable method for inheriting classes. Calling this without overriding or in override causes the animation to stop.
		/// </summary>
		/// <param name="frametime"></param>
		/// <param name="target"></param>
		protected virtual void InternalUpdate(float frametime, Transformable target)
		{
			Stop();
		}
	}
}
