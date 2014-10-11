using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
	public class Game : IDisposable
	{
		public RenderWindow Window { get; private set; }
		public RenderStates RStates { get; private set; }
		public Color BackgroundColor { get; set; }
		public float Frametime { get; private set; }
		public float FPS
		{
			get
			{
				return 1f / (Frametime * 1000f);
			}
		}

		GameState activeGameState;
		RenderTexture stateTexture;

		string title;

		public Game()
		{
			BackgroundColor = new Color(100, 149, 247);

			var ingame = new Ingame(this);
			SetActiveGameState(ingame);
		}

		public void Dispose()
		{
			Window.Dispose();
		}

		public void SetActiveGameState(GameState state)
		{
			if (!state.Loaded)
				state.Load();

			activeGameState = state;
			stateTexture = new RenderTexture((uint)activeGameState.Bounds.X, (uint)activeGameState.Bounds.Y);
		}

		public void Start(uint width, uint height, string title)
		{
			var mode = new VideoMode(width, height);
			this.title = title;

			Window = new RenderWindow(mode, title, Styles.Default);
			RStates = new RenderStates(RenderStates.Default);

			Window.Closed += Window_Closed;
			Window.Resized += Window_Resized;

			Window.SetVerticalSyncEnabled(true);

			var frametimer = new Stopwatch();
			while (Window.IsOpen())
			{
				frametimer.Restart();

				Window.DispatchEvents();

				activeGameState.Update(Frametime);

				Window.Clear(Color.Black);
				stateTexture.Clear(BackgroundColor);

				activeGameState.Draw(stateTexture, RStates);

				stateTexture.Display();
				var sprite = new Sprite(stateTexture.Texture);
				var pos = new Vector2f(Window.Size.X / 2, Window.Size.Y / 2);
				var origin = new Vector2f(stateTexture.Size.X / 2, stateTexture.Size.Y / 2);

				sprite.Position = pos;
				sprite.Origin = origin;

				var topDist = (pos - origin).Y;
				var leftDist = (pos - origin).X;
				var dist = Math.Min(topDist, leftDist);

				var scale = 1f;
				if (topDist < leftDist)
					scale = (topDist * 2f + stateTexture.Size.Y) / stateTexture.Size.Y;
				else
					scale = (leftDist * 2f + stateTexture.Size.X) / stateTexture.Size.X;

				var scaleVec = new Vector2f(scale, scale);
				sprite.Scale = scaleVec;

				sprite.Draw(Window, RStates);

				Window.Display();

				frametimer.Stop();
				Frametime = (float)frametimer.Elapsed.TotalMilliseconds;
				Window.SetTitle(String.Format("{0} ({1:00.0} ms)", title, Frametime));
			}
		}

		void Window_Resized(object sender, SizeEventArgs e)
		{
			Window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
		}

		void Window_Closed(object sender, EventArgs e)
		{
			Console.WriteLine("Window closing...");
			Window.Close();
		}
	}
}
