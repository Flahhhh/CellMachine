using System.Drawing;
using System.Dynamic;
using Models;

namespace Utils
{
    public static class GeneralUtils
    {
        public static Color GenerateRandomColor(int seed)
        {
            Random random = new Random(seed);
            return Color.FromArgb(
                Random.Shared.Next(256),
                Random.Shared.Next(256),
                Random.Shared.Next(256)
            );
        }
    }
    public static class GeneticUtils
    {
        public static Genome Mutation(int geneSize, int triggersCount, int[] gene, int[] triggers, double mutationChance, int seed = 42)
        {
            Random random = new Random(seed);

            for (int i=0; i<geneSize; i++)
            {
                if (random.NextDouble() > mutationChance) continue;

                switch (random.Next(3))
                {
                    case 0:
                        //Gene Uniform
                        gene[i] = (gene[i] + random.Next(0, geneSize)) % geneSize;
                        break;
                    case 1:
                        //Gene Inversion
                        gene[i] = geneSize - gene[i] - 1;
                        break;
                    case 2:
                        //Trigger Modification
                        triggers[i] = random.Next(triggersCount);
                        break;
                }
            }
        }

        public static Genome GenerateRandomGene(int geneSize, int triggersCount, int seed = 42, string geneType = "Fixed")
        {
            Random random = new Random(seed);

            int[] gene = new int[geneSize], triggers = new int[geneSize];

            for (int i = 0; i < geneSize; i++)
            {
                if (geneType == "Fixed")
                {
                    gene[i] = random.Next(geneSize);
                }
                else if (geneType == "Moving")
                {
                    gene[i] = random.Next(-geneSize, geneSize);
                }
                else
                {
                    throw new Exception("Ошибка: Неизвестный тип гена");
                }

                triggers[i] = random.Next(triggersCount);
            }

            return new Genome { gene = gene, triggers = triggers };
        }
    }
    public static class SimUtils
    {
        public static Pos GetShift(Orientation orientation)
        {
            Pos shift;

            switch (orientation)
            {
                case Orientation.FORWARD:
                    shift = new Pos { x = 0, y = 1 };
                    break;
                case Orientation.LEFT:
                    shift = new Pos { x = 1, y = 0 };
                    break;
                case Orientation.RIGHT:
                    shift = new Pos { x = -1, y = 0 };
                    break;
                default:
                    shift = new Pos { x = 0, y = -1 };
                    break;
            }

            return shift;
        }

        public static HashSet<Pos> GenerateUniquePoses(int n, int h, int w, int seed = 42)
        {
            Random random = new Random(seed);
            HashSet<Pos> poses = new HashSet<Pos>();
            while (poses.Count() < n)
            {
                int x = random.Next(0, w - 1);
                int y = random.Next(0, h - 1);
                poses.Add(new Pos { x = x, y = y });
            }

            return poses;
        }
    }

    public struct Genome
    {
        public int[] gene;
        public int[] triggers;
    }

    public struct Pos
    {
        public int x;
        public int y;
    }

    public enum ActionType
    {
        NONE,
        ATTACK,
        MOVE,
        LEFT,
        RIGHT,
        DIVIDE,
    }

    public enum CellCondition
    {
        IS_42,
        IS_NONE,
        IS_WALL,
        IS_ENEMY,
        IS_ALLIE,
        IS_FOOD,
        IS_HIGHT_ENERGY,
        IS_LOW_ENERGY,
        IS_HIGHT_HP,
        IS_LOW_HP,
    }

    public enum CellViewObject
    {
        NONE,
        WALL,
        ENEMY,
        ALLIE,
        FOOD,
    }

    public enum Orientation
    {
        FORWARD,
        LEFT,
        RIGHT,
        BACKWARD,
    }
}