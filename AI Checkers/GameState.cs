using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public abstract class GameState
	{
		public virtual Vector2f Bounds
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool Loaded { get; private set; }

		protected Game game;

		public GameState(Game game)
		{
			this.game = game;
		}

		public virtual void Load()
		{
			Loaded = true;
		}

		public virtual void Update(float frametime)
		{

		}

		public virtual void Draw(RenderTarget target, RenderStates states)
		{

		}
	}
}
