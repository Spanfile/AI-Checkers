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
        Tuple<int, int> move;

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

            aiProcess.BeginOutputReadLine();
            aiProcess.OutputDataReceived += aiProcess_OutputDataReceived;

            Console.WriteLine("{0}: AI process started (\"{1}\", \"{2}\")", color, fullAiPath, args);
        }

        void aiProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("{0} AI: \"{1}\"", PlayerColor, e.Data);

            if (String.IsNullOrWhiteSpace(e.Data))
                return;

            if (e.Data.StartsWith("mv:"))
            {
                var moveArgs = e.Data.Substring(3).Split(' ');

                var piece = -1;
                var to = -1;
                Int32.TryParse(moveArgs[0], out piece);
                Int32.TryParse(moveArgs[1], out to);

                move = new Tuple<int, int>(piece, to);

                state = PlayerState.Finished;
            }
        }

        void WriteToAI(string command)
        {
            aiProcess.StandardInput.WriteLine(command);
            aiProcess.StandardInput.Flush();
        }

        public override void StartMove()
        {
            base.StartMove();

            var move = new MoveInfo();
            move.SetMap(GameState.Ingame.tiles, GameState.Ingame.pieces);
            string info = move.Serialize();

            Console.WriteLine("Sending map info to {0}: {1}", PlayerColor, info);
            WriteToAI(info);
        }

        public override Tuple<int, int> GetMove()
        {
            if (move != null)
                return move;

            return base.GetMove();
        }

        public override void Stop()
        {
            WriteToAI("exit");
        }
    }
}
