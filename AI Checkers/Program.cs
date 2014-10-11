using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Checkers
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var game = new Game())
			{
				game.Start(1200, 675, "AI Checkers");
			}
		}
	}
}
