using Const;

namespace Utils
{
    public static class GeneticUtils
    {
        public static Genome Mutation(int geneSize, int triggersCount, int[] gene, int[] triggers, double mutationChance)
        {
            int[] newGene = gene.ToArray(), newTriggers = triggers.ToArray();

            for (int i=0; i<geneSize; i++)
            {
                if (Constants.random.NextDouble() > mutationChance) continue;

                switch (Constants.random.Next(3))
                {
                    case 0:
                        //Gene Uniform
                        newGene[i] = (newGene[i] + Constants.random.Next(0, geneSize)) % geneSize;
                        break;
                    case 1:
                        //Gene Inversion
                        newGene[i] = geneSize - newGene[i] - 1;
                        break;
                    case 2:
                        //Trigger Modification
                        newTriggers[i] = Constants.random.Next(triggersCount);
                        break;
                }
            }

            return new Genome{gene=newGene, triggers=newTriggers};
        }

        public static Genome GenerateRandomGene(int geneSize, int triggersCount, string geneType = "Fixed")
        {

            int[] gene = new int[geneSize], triggers = new int[geneSize];

            for (int i = 0; i < geneSize; i++)
            {
                if (geneType == "Fixed")
                {
                    gene[i] = Constants.random.Next(geneSize);
                }
                //else if (geneType == "Moving")
                //{
                //    gene[i] = Constants.random.Next(-geneSize, geneSize);
                //}
                else
                {
                    throw new Exception("Ошибка: Неизвестный тип гена");
                }

                triggers[i] = Constants.random.Next(triggersCount);
            }

            return new Genome { gene = gene, triggers = triggers };
        }
    }

    public struct Genome
    {
        public int[] gene;
        public int[] triggers;
    }
}