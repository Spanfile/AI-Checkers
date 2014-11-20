using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
    public abstract class Player
    {
        public PieceType PlayerColor { get; private set; }

        protected Game game;

        public Player(Game game, PieceType color)
        {
            this.game = game;
            PlayerColor = color;
            
        }

        public virtual Task<Tuple<int, int>> GetMove()
        {
            return null;
        }

        public virtual void Update(float frametime)
        {

        }
    }
}
