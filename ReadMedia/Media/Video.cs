using System;
using NReco.VideoInfo;

namespace ReadMedia.Media
{
    public class FFFProbeProvider
    {
        public FFProbe Val { private set; get; }
        public FFFProbeProvider()
        {
            Val = new FFProbe();
        }
    }
    public class Video : MyFileInfo
    {
        private readonly int width;
        private readonly int height;
        public Video(string path, FFProbe probe) : base(path)
        {
            MediaInfo mediaInfo = probe.GetMediaInfo(FullName);
            Duration = mediaInfo.Duration;
            VideoCodecFullName = mediaInfo.Streams[0].CodecLongName;
            VideoCodecName = mediaInfo.Streams[0].CodecName;
            PixelFormat = mediaInfo.Streams[0].PixelFormat;
            FrameRate = mediaInfo.Streams[0].FrameRate;
            width = mediaInfo.Streams[0].Width;
            height = mediaInfo.Streams[0].Height;
            try
            {
                MediaInfo.StreamInfo info = mediaInfo.Streams[1];
                AudioCodecFullName = info.CodecLongName;
                AudioCodecName = info.CodecName;
            }
            catch (Exception e)
            {
                AudioCodecFullName = string.Empty;
                AudioCodecName = string.Empty;
            }
        }
        public TimeSpan Duration { private set; get; }
        public string VideoCodecFullName { private set; get; }
        public string VideoCodecName { private set; get; }
        public string PixelFormat { private set; get; }
        public float FrameRate { private set; get; }
        public override int Width => width;
        public override int Height => height;
        public string AudioCodecFullName { private set; get; }
        public string AudioCodecName { private set; get; }
        public override string ConsoleDisplay(){
            return base.ConsoleDisplay() + $"\n\t{Width}x{Height}\n\t{Duration}\n\tVideo Codec: {VideoCodecName} ({VideoCodecFullName})" +
                $"\n\tAudio Codec: {AudioCodecName} ({AudioCodecFullName})\n\tFrameRate: {FrameRate}\n\tPixel Format: {PixelFormat}";
        }
        public new static string CSVHeader()
        {
            return MyFileInfo.CSVHeader() + ",Duration,VideoCodecFullName,VideoCodecName,PixelFormat,FrameRate,AudioCodecFullName,AudioCodecName";
        }
        public override string CSVregister()
        {
            return base.CSVregister() + $",{Duration},{VideoCodecFullName},{VideoCodecName},{PixelFormat},{FrameRate},{AudioCodecFullName},{AudioCodecName}";
        }
    }
}
