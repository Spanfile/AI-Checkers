﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers.UI
{
	public class Label : UIObject
	{
		Text text;

		public Label(Game game, Vector2f position, string text, Font font, Color color, uint size)
			: base(game, position)
		{
			this.text = new Text(text, font, size);
			this.text.Color = color;
			this.text.Position = new Vector2f((int)-(this.text.GetLocalBounds().Width / 2), (int)-(this.text.GetLocalBounds().Height / 2));
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			text.Draw(target, states);

			base.Draw(target, states);
		}
	}
}
