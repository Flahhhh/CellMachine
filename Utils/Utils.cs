
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Dynamic;
using Const;
using Models;
using System.Diagnostics;

namespace Utils
{
    public static class GeneralUtils
    {
        public static Color GenerateRandomColor()
        {
            return Color.FromRgb(
                (byte)Constants.random.Next(20, 256),
                0,//(byte)Constants.random.Next(20,256),
                (byte)Constants.random.Next(20,256)
            );
        }
    }
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

        public static HashSet<Pos> GenerateUniquePoses(int n, int h, int w)
        {
            HashSet<Pos> poses = new HashSet<Pos>();
            while (poses.Count() < n)
            {
                int x = Constants.random.Next(w);
                int y = Constants.random.Next(h);
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
        HEAL,
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

    public class VideoRecorder
    {
        private string outputPath;
        private int width, height;
        private int cellSize;
        private int fps;
        private ProcessStartInfo processStartInfo;
        private Process? _ffmpegProcess;
        private Stream? _ffmpegInputStream;
        private byte[]? _pixelBuffer;

        public VideoRecorder(string outputPath, int fps, int cellSize, int width, int height)
        {
            this.outputPath = outputPath;
            this.fps = fps;
            this.width = width; 
            this.height = height;
            this.cellSize = cellSize;
        }

        public void InitVideoRecorder()
        {
            int videoWidth = width * cellSize;
            int videoHeight = height * cellSize;

            _pixelBuffer = new byte[videoWidth * videoHeight * 3];

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg", 
                Arguments = $"-y -f rawvideo -pix_fmt rgb24 -s {videoWidth}x{videoHeight} -r {fps} -i - -c:v libx264 -pix_fmt yuv420p \"{outputPath}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _ffmpegProcess = Process.Start(startInfo);
            _ffmpegInputStream = _ffmpegProcess?.StandardInput.BaseStream;
        }

        public bool CheckStatus()
        {
            return _ffmpegInputStream == null || _pixelBuffer == null;
        }

        public void CopyToBuffer(Image<Rgb24> frameImage)
        {
            frameImage.CopyPixelDataTo(_pixelBuffer);
        }

        public void SaveBuffer()
        {
            _ffmpegInputStream.Write(_pixelBuffer, 0, _pixelBuffer.Length);
            _ffmpegInputStream.Flush();
        }

        public void CloseVideoRecorder()
        {
            if (_ffmpegInputStream != null)
            {
                _ffmpegInputStream.Close();
                _ffmpegInputStream = null;
            }

            if (_ffmpegProcess != null)
            {
                _ffmpegProcess.WaitForExit();
                _ffmpegProcess.Dispose();
                _ffmpegProcess = null;
            }
        }
    }
}