namespace Config
{
    public static class GameConfig
    {
        public static int randomSeed = 42;
        public static string geneType = "Fixed"; // Moving || Fixed
        public static int actionSpace;
        public static int stepPeriod;
        public static int geneSize;
        public static int triggersCount;
        public static int initColoniesCount;
        public static int maxHP;
        public static double generationEnergy;
        public static double cellHealFactor;
        public static double cellEnergyDecayingFactor;
        public static double afterCellFoodAmount;
        public static double foodDecayingFactor;
    }
}