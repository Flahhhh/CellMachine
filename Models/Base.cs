using System.Diagnostics.Contracts;
using Config;
using Utils;

namespace Models
{
    public class Tile
    {
        virtual public ActionType Step(CellViewObject cellViewObject)
        {
            return ActionType.NONE;
        }
        virtual public ActionType Step()
        {
            return ActionType.NONE;
        }
        virtual public Dictionary<String, object> ToJson(int y, int x)
        {
            throw new NotImplementedException();
        }
        public Tile() { }
    }
}