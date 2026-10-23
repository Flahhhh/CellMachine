namespace Config
{
    public static class GameConfig
    {
        public static int randomSeed = 4242;
        public static string geneType = "Fixed"; // Moving || Fixed
        public static int actionSpace = 7;
        public static int simIterations = 10000;
        public static int saveFreq = 2000;
        public static int stepPeriod = 5;
        public static double genomeMutationChance = 0.01;
        public static double geneMutationChance = 0.02;
        public static int geneSize = 256;
        public static int triggersCount = 10;
        public static int initColoniesCount = 50000;
        public static double maxHP = 100;
        public static double damage = 15;
        public static double maxEnergy = 100;
        public static double maxLiveEnergyCost = 0.5;
        public static double attackEnergyCost = 5;
        public static double moveEnergyCost = 1;
        public static double generationEnergy = 100;
        public static double cellHealFactor = 0.5;
        public static double foodDecayingFactor = 0.00005;
        public static double foodSpawnChance = 0.0005;
        public static double foodSpawnEnergyAmount = 50;
    }
}