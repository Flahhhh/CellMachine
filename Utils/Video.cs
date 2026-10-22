using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Diagnostics;

namespace Utils
{
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