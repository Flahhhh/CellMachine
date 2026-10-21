using Config;
using Const;
using Utils;

namespace Models
{
    class Cell
    {
        public int age;
        public double energy;
        public int hp;
        public int colonyID;
        public int? ancestorColonyID;
        public Colony colony;
        private int genePointer;
        private Orientation orientation;

        public Cell(Colony colony, double energy)
        {
            genePointer = 0;
            hp = GameConfig.maxHP;

            this.ancestorColonyID = colony.ancestorID;
            this.colonyID = colony.id;
            this.colony = colony;
            this.energy = energy;
        }

        public bool isAlive()
        {
            return hp > 0 && energy > 0;
        }

        public Orientation getOrientation()
        {
            return orientation;
        }

        public ActionType step(CellViewObject cellViewObject)
        {
            int curGene = colony.gene[genePointer];
            int curTrigger = colony.triggers[genePointer];
            bool isCondition = checkCondition(cellViewObject, (CellCondition)curTrigger);
            if (isCondition)
            {
                if (GameConfig.geneType == "Moving")
                {
                    genePointer = (genePointer + curGene) % GameConfig.geneSize;
                }
                else if (GameConfig.geneType == "Fixed")
                {
                    genePointer = curGene;
                }
                else
                {
                    throw new Exception("Ошибка: Неизвестный тип гена");
                }

                ActionType actionType = (ActionType)(genePointer % GameConfig.actionSpace);
                return actionType;
            }
            else
            {
                genePointer = (genePointer + 1) % GameConfig.geneSize;
                return ActionType.NONE;
            }
        }

        private bool checkCondition(CellViewObject cellViewObject, CellCondition cellCondition)
        {
            if (cellCondition == CellCondition.IS_42)
            {
                return true;
            }
            else if (cellCondition == CellCondition.IS_NONE)
            {
                return cellViewObject == CellViewObject.NONE;
            }
            else if (cellCondition == CellCondition.IS_WALL)
            {
                return cellViewObject == CellViewObject.WALL;
            }
            else if (cellCondition == CellCondition.IS_ENEMY)
            {
                return cellViewObject == CellViewObject.ENEMY;
            }
            else if (cellCondition == CellCondition.IS_ALLIE)
            {
                return cellViewObject == CellViewObject.ALLIE;
            }
            else if (cellCondition == CellCondition.IS_FOOD)
            {
                return cellViewObject == CellViewObject.FOOD;
            }
            else if (cellCondition == CellCondition.IS_HIGHT_ENERGY)
            {
                return hp >= GameConfig.maxHP * 0.75;
            }
            else if (cellCondition == CellCondition.IS_LOW_ENERGY)
            {
                return hp <= GameConfig.maxHP * 0.25;
            }
            else
            {
                return true;
            }
        }
    }
}