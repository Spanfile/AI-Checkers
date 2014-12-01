using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AI_Checkers
{
    public class MoveInfo
    {
        bool useLineSeparator = true;
        bool usePieceSeparator = true;
        Tuple<int, int>[] map;

        public MoveInfo()
        {
            useLineSeparator = Config.GetBool("useLineSeparator");
            usePieceSeparator = Config.GetBool("usePieceSeparator");
        }

        public void SetMap(List<Tile> tiles, List<Piece> pieces)
        {
            map = (from tile in tiles
                   let piece = pieces.Find(p => p.boardPos.Equals(tile.boardPos))
                   let type = piece == null ? 0 : piece.color == PieceColor.Red ? 1 : 2
                   let super = piece == null ? 0 : piece.super ? 1 : 0
                   select Tuple.Create<int, int>(type, super)).ToArray();
        }

        public string Serialize()
        {
            return String.Join(useLineSeparator ? "|" : "", MapEnum());
        }

        IEnumerable<string> MapEnum()
        {
            int index = 0;
            while (index < map.Length / 8)
            {
                yield return String.Join("", map.Skip(index * 8).Take(8).Select(t => String.Format("{0}{1}{2}", t.Item1, t.Item2, usePieceSeparator ? "/" : "")));
                index += 1;
            }
        }
    }
}
