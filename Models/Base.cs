using System.Diagnostics.Contracts;
using Config;
using Utils;

namespace Models
{
    class Tile
    {
        public ActionType step(CellViewObject cellViewObject)
        {
            return ActionType.NONE;
        }
        public ActionType step()
        {
            return ActionType.NONE;
        }
        public Tile() {}
    }
}