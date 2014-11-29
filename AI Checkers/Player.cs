using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace AI_Checkers
{
    public enum PlayerState
    {
        Idle,
        Running,
        Finished
    }

    public abstract class Player
    {
        public PieceColor PlayerColor { get; private set; }

        protected Game game;
        protected PlayerState state;

        public Player(Game game, PieceColor color)
        {
            this.game = game;
            PlayerColor = color;
            state = PlayerState.Idle;
        }

        public virtual void StartMove()
        {
            state = PlayerState.Running;
        }

        public virtual PlayerState GetState()
        {
            return state;
        }

        public virtual Tuple<int, int> GetMove()
        {
            return null;
        }

        public virtual void Stop()
        {

        }

        public virtual void Update(float frametime)
        {

        }
    }
}
