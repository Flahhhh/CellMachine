namespace Config
{
    public static class GameConfig
    {
        public static int randomSeed = 42;
        public static string geneType = "Fixed"; // Moving || Fixed
        public static int actionSpace;
        public static int stepPeriod;
        public static double genomeMutationChance;
        public static double geneMutationChance;
        public static int geneSize;
        public static int triggersCount;
        public static int initColoniesCount;
        public static double maxHP;
        public static double damage;
        public static double maxEnergy;
        public static double maxLiveEnergyCost;
        public static double attackEnergyCost;
        public static double moveEnergyCost;
        public static double generationEnergy;
        public static double cellHealFactor;
        public static double cellEnergyDecayingFactor;
        public static double afterCellFoodAmount;
        public static double foodDecayingFactor;
    }
}