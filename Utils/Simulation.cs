using Const;

namespace Utils
{
    public static class SimUtils
    {
        public static Pos GetShift(Orientation orientation)
        {
            Pos shift;

            switch (orientation)
            {
                case Orientation.FORWARD:
                    shift = new Pos { x = 0, y = 1 };
                    break;
                case Orientation.LEFT:
                    shift = new Pos { x = 1, y = 0 };
                    break;
                case Orientation.RIGHT:
                    shift = new Pos { x = -1, y = 0 };
                    break;
                default:
                    shift = new Pos { x = 0, y = -1 };
                    break;
            }

            return shift;
        }

        public static HashSet<Pos> GenerateUniquePoses(int n, int h, int w)
        {
            HashSet<Pos> poses = new HashSet<Pos>();
            while (poses.Count() < n)
            {
                int x = Constants.random.Next(w);
                int y = Constants.random.Next(h);
                poses.Add(new Pos { x = x, y = y });
            }

            return poses;
        }
    }

    public struct Pos
    {
        public int x;
        public int y;
    }

    public enum ActionType
    {
        NONE,
        ATTACK,
        MOVE,
        LEFT,
        RIGHT,
        DIVIDE,
        HEAL,
    }

    public enum CellCondition
    {
        IS_42,
        IS_NONE,
        IS_WALL,
        IS_ENEMY,
        IS_ALLIE,
        IS_FOOD,
        IS_HIGHT_ENERGY,
        IS_LOW_ENERGY,
        IS_HIGHT_HP,
        IS_LOW_HP,
    }

    public enum CellViewObject
    {
        NONE,
        WALL,
        ENEMY,
        ALLIE,
        FOOD,
    }

    public enum Orientation
    {
        FORWARD,
        LEFT,
        RIGHT,
        BACKWARD,
    }
}