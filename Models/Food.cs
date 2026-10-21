using Config;
using Utils;

namespace Models
{
    class Food: Tile
    {
        private double energy;

        public Food(double energy)
        {
            this.energy = energy;
        }

        public double GetEnergy()
        {
            return energy;
        }

        override public ActionType Step()
        {
            energy *= GameConfig.foodDecayingFactor;
            return ActionType.NONE;
        }

    }
}