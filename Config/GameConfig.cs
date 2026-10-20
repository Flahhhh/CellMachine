namespace Config
{
    public record class GameConfig
    {
        public string geneType = "Shift"; // Shift || Fixed
        public int actionSpace;
        public int stepPeriod;
        public int geneSize;
        public int maxHP;
        public double cellHealFactor;
        public double cellEnergyDecayingFactor;
        public double afterCellFoodAmount;
        public double foodDecayingFactor;
    }
}