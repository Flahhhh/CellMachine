namespace Config
{
    public static class GameConfig
    {
        public static int randomSeed = 42;
        public static string geneType = "Fixed"; // Moving || Fixed
        public static int actionSpace = 7;
        public static int stepPeriod = 5;
        public static double genomeMutationChance = 0.001;
        public static double geneMutationChance = 0.02;
        public static int geneSize = 256;
        public static int triggersCount = 10;
        public static int initColoniesCount = 10000;
        public static double maxHP = 100;
        public static double damage = 20;
        public static double maxEnergy = 100;
        public static double maxLiveEnergyCost = 1;
        public static double attackEnergyCost = 5;
        public static double moveEnergyCost = 2;
        public static double generationEnergy = 40;
        public static double cellHealFactor = 0.5;
        public static double cellEnergyDecayingFactor = 0.02;
        public static double afterCellFoodAmount = 30;
        public static double foodDecayingFactor = 0.001;
        public static double foodSpawnChance = 0.005;
        public static double foodSpawnEnergyAmount = 10;
    }
}