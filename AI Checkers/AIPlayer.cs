using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AI_Checkers
{
    public class AIPlayer : Player
    {
        Process aiProcess;

        public AIPlayer(Game game, PieceColor color, string fullAiPath, string args)
            : base(game, color)
        {
            aiProcess = new Process();
            var startInfo = new ProcessStartInfo(fullAiPath, args);

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            aiProcess.StartInfo = startInfo;
            aiProcess.Start();

            Console.WriteLine("{0}: AI process started (\"{1}\", \"{2}\")", color, fullAiPath, args);
        }

        void WriteToAI(string command)
        {
            aiProcess.StandardInput.WriteLine(command);
            aiProcess.StandardInput.Flush();
        }

        public override void StartMove()
        {
            var move = new MoveInfo();
            move.SetMap(GameState.Ingame.tiles, GameState.Ingame.pieces);
            string info = move.Serialize();

            Console.WriteLine(info);

            base.StartMove();
        }

        public override Tuple<int, int> GetMove()
        {
            return base.GetMove();
        }

        public override void Stop()
        {
            
        }
    }
}
