using Config;
using Utils;

namespace Const {
    public static class Constants
    {
        public static Random random = new Random(GameConfig.randomSeed);
        public static VideoRecorder videoRecorder = new VideoRecorder(VideoConfig.outputPath, VideoConfig.fps, VideoConfig.cellSize, BoardConfig.width, BoardConfig.height);
    }
}