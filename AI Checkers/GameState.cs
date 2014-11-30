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
        public static MainMenu MainMenu;
        public static Ingame Ingame;
        public static EndMenu EndMenu;

        public bool Active { get; set; }

        public virtual Vector2f Bounds
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Loaded { get; private set; }

        protected Game game;

        public GameState()
        {
        }
        static GameState()
        {
            MainMenu = new MainMenu();
            Ingame = new Ingame();
            EndMenu = new EndMenu();
        }

        public virtual void Load(Game game)
        {
            this.game = game;
            Loaded = true;
        }

        public virtual void Close()
        {

        }

        public virtual void Update(float frametime)
        {

        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {

        }
    }
}
