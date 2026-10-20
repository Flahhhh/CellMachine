using Config;
using Const;
using Utils;

namespace Models
{
    class Cell {
        public double energy;
        public int hp;
        public int colonyID;
        public int? ancestorColonyID;
        public Colony colony;
        private int genePointer;

        public Cell(Colony colony, double energy)
        {
            genePointer = 0;
            hp = Settings.gameConfig.maxHP;

            this.ancestorColonyID = colony.ancestorID;
            this.colonyID = colony.id;
            this.colony = colony;
            this.energy = energy;
        }

        public ActionType step(CellViewObject cellViewObject)
        {
            int curGene = colony.gene[genePointer];

            if (curGene < Settings.gameConfig.actionSpace)
            {
                genePointer = (genePointer + 1) % Settings.gameConfig.geneSize;
                return (ActionType)curGene;
            } else
            {
                bool isCondition = checkCondition((CellCondition)(curGene - Settings.gameConfig.actionSpace + 1));
                if (isCondition)
                {
                    ActionType actionType = (ActionType)(genePointer % Settings.gameConfig.actionSpace);
                    genePointer = (genePointer + 1) % Settings.gameConfig.geneSize;
                    
                    return actionType;
                } else
                {
                    genePointer = (genePointer+1) % Settings.gameConfig.geneSize;
                }

            }
        }

        private bool checkCondition(CellViewObject cellViewObject, CellCondition cellCondition)
        {
            if (cellCondition == CellCondition.IS_NONE)
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
            } else if()
        }
    }
}