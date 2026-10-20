using Models;

namespace Utils
{
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
    }

    public enum CellCondition
    {
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
}