using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers.UI
{
	public abstract class UIObject : Transformable, Drawable
	{
		public UIObject parent;
		public List<UIObject> children;

		public Vector2f size;
		public FloatRect Bounds
		{
			get
			{
				if (parent == null)
					return new FloatRect(Position.X, Position.Y, size.X, size.Y);

				return new FloatRect(parent.Bounds.Left + Position.X, parent.Bounds.Top + Position.Y, size.X, size.Y);
			}
		}

		protected Game game;

		public UIObject(Game game, Vector2f position, Vector2f size)
		{
			Position = position;
			this.size = size;
			this.game = game;

			children = new List<UIObject>();
		}
		public UIObject(Game game, Vector2f position)
			: this(game, position, new Vector2f(0, 0))
		{

		}

		public void AddChild(UIObject child)
		{
			child.parent = this;
			children.Add(child);
		}

		public void RemoveChild(UIObject child)
		{
			children.Remove(child);
			child.Dispose();
		}

		public void Remove()
		{
			if (parent != null)
				parent.RemoveChild(this);
		}

		public virtual void Update(float frametime)
		{
			foreach (var child in children)
				child.Update(frametime);
		}

		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			foreach (var child in children)
				child.Draw(target, states);
		}
	}
}
