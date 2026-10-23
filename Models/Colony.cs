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
        public Colony? ancestor;
        private static int NextID { get; set; } = 1;

        public long localRelatives = 0;
        public int[] gene = new int[GameConfig.geneSize];
        public int[] triggers = new int[GameConfig.geneSize];
        public bool IsAlive
        {
            get
            {
                return localRelatives > 0;
            }
        }

        public Colony(Genome genome, Color color, int? ancestorID = null, Colony? ancestor = null)
        {
            id = NextID;
            NextID++;

            this.gene = genome.gene;
            this.triggers = genome.triggers;

            this.ancestorID = ancestorID;
            this.ancestor = ancestor;
            this.color = color;
        }
        public Colony(Color color, int? ancestorID = null, Colony? ancestor = null) : this(
            GeneticUtils.GenerateRandomGene(GameConfig.geneSize, GameConfig.triggersCount, GameConfig.geneType),
            color,
            ancestorID,
            ancestor)
        { }
        public Colony() : this(GeneralUtils.GenerateRandomColor()) { }

        public Colony GenerateNewColony()
        {
            Genome newGenome = GeneticUtils.Mutation(GameConfig.geneSize, GameConfig.triggersCount, gene, triggers, GameConfig.geneMutationChance);
            Color newColor = SelectNewColor();
            int newAncestorID;
            Colony newAncestor;

            if (this.ancestorID == null)
            {
                newAncestorID = this.id;
                newAncestor = this;
            }
            else
            {
                newAncestorID = (int)this.ancestorID;
                newAncestor = this.ancestor!;
            }

            return new Colony(newGenome, newColor, newAncestorID, newAncestor);
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

        public Dictionary<string, object> ToJson()
        {
            Dictionary<String, object> toSave = new Dictionary<string, object>();
            toSave["id"] = id;
            toSave["ancestor_id"] = ancestorID!=null ? ancestorID : -1;
            toSave["localRelatives"] = localRelatives;
            toSave["gene"] = gene;
            toSave["triggers"] = triggers;
            toSave["color"] = color.ToString();

            return toSave;
        }
    }
}