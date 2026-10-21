using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Config;
using Const;
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
            GeneticUtils.GenerateRandomGene(GameConfig.geneSize, GameConfig.triggersCount, GameConfig.geneType),
            color,
            ancestorID)
        { }
        public Colony() : this(GeneralUtils.GenerateRandomColor()) { }

        public Colony GenerateNewColony()
        {
            Genome newGenome = GeneticUtils.Mutation(GameConfig.geneSize, GameConfig.triggersCount, gene, triggers, GameConfig.geneMutationChance);
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
            Rgba32 currentRgba = color.ToPixel<Rgba32>();
            return Color.FromRgb(
                (byte)Math.Max(20, Math.Min(255, currentRgba.R + Constants.random.Next(-25, 25))), 
                0,//(byte)Math.Max(20, Math.Min(255, currentRgba.G + Constants.random.Next(-25, 25))), 
                (byte)Math.Max(0, Math.Min(255, currentRgba.B + Constants.random.Next(-25, 25)))
            );
        }
    }
}