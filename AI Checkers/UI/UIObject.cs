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

		public UIObject(Vector2f position)
		{
			Position = position;

			children = new List<UIObject>();
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
