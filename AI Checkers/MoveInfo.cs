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
        byte[] map;

        public MoveInfo()
        {

        }

        public void SetMap(List<Tile> tiles, List<Piece> pieces)
        {
            map = (from tile in tiles
                   let piece = pieces.Find((p) => p.boardPos.Equals(tile.boardPos))
                   select piece == null ? (byte)0 : piece.color == PieceColor.Red ? (byte)1 : (byte)2).ToArray();
        }

        public string Serialize()
        {
            return String.Join("|", MapEnum());
        }

        IEnumerable<string> MapEnum()
        {
            int index = 0;
            while (index < map.Length / 8)
            {
                yield return String.Join("", map.Skip(8 * index).Take(8));
                index += 1;
            }
        }
    }
}
