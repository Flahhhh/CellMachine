using Config;
using Const;
using Utils;

namespace Models
{
    class Cell {
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
            hp = Settings.gameConfig.maxHP;

            this.ancestorColonyID = colony.ancestorID;
            this.colonyID = colony.id;
            this.colony = colony;
            this.energy = energy;
        }

        public bool isAlive()
        {
            return hp > 0 && energy > 0;
        }

        override public Orientation getOrientation()
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
                if (Settings.gameConfig.geneType == "Shift")
                {
                    genePointer = (genePointer + curGene) % Settings.gameConfig.geneSize;
                } else if(Settings.gameConfig.geneType == "Fixed")
                {
                    genePointer = curGene;
                } else
                {
                    throw new ValueError();
                }

                ActionType actionType = (ActionType)(genePointer % Settings.gameConfig.actionSpace);   
                return actionType;
            } else
            {
                genePointer = (genePointer+1) % Settings.gameConfig.geneSize;
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
            } else if (cellCondition == CellCondition.IS_WALL)
            {
                return cellViewObject == CellViewObject.WALL;
            } else if(cellCondition == CellCondition.IS_ENEMY)
            {
                return cellViewObject == CellViewObject.ENEMY;
            } else if(cellCondition == CellCondition.IS_ALLIE)
            {
                return cellViewObject == CellViewObject.ALLIE;
            } else if(cellCondition == CellCondition.IS_FOOD)
            {
                return cellViewObject == CellViewObject.FOOD;
            } else if(cellCondition == CellCondition.IS_HIGHT_ENERGY)
            {
                return hp >= Settings.gameConfig.maxHP*0.75;
            } else if(cellCondition == CellCondition.IS_LOW_ENERGY)
            {
                return hp <= Settings.gameConfig.maxHP*0.25;
            }
        }
    }
}