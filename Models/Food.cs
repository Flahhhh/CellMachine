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
            energy -= GameConfig.foodSpawnEnergyAmount * GameConfig.foodDecayingFactor;
            return ActionType.NONE;
        }

        public override Dictionary<string, object> ToJson(int y, int x)
        {
            Dictionary<String, object> toSave = new Dictionary<string, object>();
            toSave["type"] = "Food";
            toSave["energy"] = energy;
            toSave["pos"] = Tuple.Create(y, x);

            return toSave;
        }

    }
}