using Config;
using Utils;
using Const;

namespace Models
{
    class Food
    {
        private double energy;

        public Food(double energy)
        {
            this.energy = energy;
        }

        public double getEnergy()
        {
            return energy;
        }

        public ActionType step(CellViewObject cellViewObject)
        {
            energy *= Settings.gameConfig.foodDecayingFactor;
            return ActionType.NONE;
        }

    }
}