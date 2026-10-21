using System.Drawing;
using Config;
using Utils;

namespace Models
{
    class Colony
    {
        public Color color;
        public int id;
        public int? ancestorID;
        private static int NextID { get; set; } = 1;

        public long descendantCount = 0;
        public long heirCount = 0;
        public int[] gene = new int[GameConfig.geneSize];
        public int[] triggers = new int[GameConfig.geneSize];

        public Colony(Genome genome, Color color, int? ancestorID = null)
        {
            id = NextID;
            NextID++;

            this.gene = genome.gene;
            this.triggers = genome.triggers;

            this.ancestorID = ancestorID;
            this.color = color;
        }
        public Colony(Color color, int? ancestorID = null) : this(
            GeneticUtils.GenerateRandomGene(GameConfig.geneSize, GameConfig.triggersCount, GameConfig.randomSeed),
            color,
            ancestorID)
        { }
        public Colony() : this(GeneralUtils.GenerateRandomColor(GameConfig.randomSeed)) { }

        public Colony GenerateNewColony()
        {
            Genome newGenome = GeneticUtils.Mutation(GameConfig.geneSize, GameConfig.triggersCount, gene, triggers, GameConfig.genomeMutationChance, GameConfig.randomSeed);
            Color newColor = SelectNewColor();
            int newAncestorID;

            if (this.ancestorID == null)
            {
                newAncestorID = this.id;
            }
            else
            {
                newAncestorID = (int)this.ancestorID;
            }

            return new Colony(newGenome, newColor, newAncestorID);
        }

        public Color SelectNewColor()
        {
            Random random = new Random(GameConfig.randomSeed);
            return Color.FromArgb(
                Math.Max(0, Math.Min(255, color.R + random.Next(-25, 25))), 
                Math.Max(0, Math.Min(255, color.G + random.Next(-25, 25))), 
                Math.Max(0, Math.Min(255, color.B + random.Next(-25, 25)))
            );
        }
    }
}