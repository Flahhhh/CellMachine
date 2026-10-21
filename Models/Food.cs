using Config;
using Utils;

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

        public ActionType step()
        {
            energy *= GameConfig.foodDecayingFactor;
            return ActionType.NONE;
        }

    }
}