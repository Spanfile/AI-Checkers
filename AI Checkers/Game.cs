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
        Vector2f stateTexPos;
        Vector2f stateTexOrigin;
        Vector2f stateTexScale;

        string title;

        public Game()
        {
            BackgroundColor = new Color(100, 149, 247);

            stateTexPos = new Vector2f(0, 0);
            stateTexScale = new Vector2f(1f, 1f);
        }

        public void Dispose()
        {
            Window.Dispose();
        }

        public void SetActiveGameState(GameState state)
        {
            if (activeGameState != null)
                activeGameState.Active = false;

            if (!state.Loaded)
                state.Load(this);

            activeGameState = state;
            state.Active = true;
            stateTexture = new RenderTexture((uint)activeGameState.Bounds.X, (uint)activeGameState.Bounds.Y);
        }

        public Vector2i GetMousePosition() // TODO: fix scale thing
        {
            var mouse = Mouse.GetPosition(Window);
            var texPos = stateTexPos - stateTexOrigin;
            var scaled = new Vector2f((stateTexPos.X - texPos.X) * stateTexScale.X, (stateTexPos.Y - texPos.Y) * stateTexScale.Y);
            var disp = stateTexPos - scaled;
            //Console.WriteLine(disp);
            return new Vector2i((int)(mouse.X - disp.X), (int)(mouse.Y - disp.Y));
        }

        void UpdateStateTexturePos()
        {
            var pos = new Vector2f(Window.Size.X / 2f, Window.Size.Y / 2f);
            var origin = new Vector2f(stateTexture.Size.X / 2f, stateTexture.Size.Y / 2f);

            var topDist = (pos - origin).Y;
            var leftDist = (pos - origin).X;

            var scale = 1f;
            if (topDist < leftDist)
                scale = (topDist * 2f + stateTexture.Size.Y) / stateTexture.Size.Y;
            else
                scale = (leftDist * 2f + stateTexture.Size.X) / stateTexture.Size.X;

            stateTexPos = pos;
            stateTexOrigin = origin;
            stateTexScale = new Vector2f(scale, scale);
        }

        public void Start(uint width, uint height, string title)
        {
            var mode = new VideoMode(width, height);
            this.title = title;

            Window = new RenderWindow(mode, title, Styles.Close);
            RStates = new RenderStates(RenderStates.Default);

            Window.Closed += Window_Closed;
            Window.Resized += Window_Resized;

            Window.SetVerticalSyncEnabled(true);

            SetActiveGameState(GameState.MainMenu);

            var frametimer = new Stopwatch();
            while (Window.IsOpen())
            {
                frametimer.Restart();

                Window.DispatchEvents();

                activeGameState.Update(Frametime);
                UpdateStateTexturePos();

                Window.Clear(Color.Black);
                stateTexture.Clear(BackgroundColor);

                activeGameState.Draw(stateTexture, RStates);

                stateTexture.Display();
                var sprite = new Sprite(stateTexture.Texture);
                sprite.Position = stateTexPos;
                sprite.Origin = stateTexOrigin;
                sprite.Scale = stateTexScale;

                sprite.Draw(Window, RStates);

                Window.Display();

                frametimer.Stop();
                Frametime = (float)frametimer.Elapsed.TotalMilliseconds;
                Window.SetTitle(String.Format("{0} ({1:00.0} ms)", title, Frametime));
            }

            GameState.Ingame.Close();
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
