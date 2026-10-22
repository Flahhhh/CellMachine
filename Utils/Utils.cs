
using SixLabors.ImageSharp;
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
}