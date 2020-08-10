using System;
using NReco.VideoInfo;

namespace ReadMedia.Media
{
    public class Video : MyFileInfo
    {
        private MediaInfo GetMediaInfo() => new FFProbe().GetMediaInfo(FullName);
        private MediaInfo.StreamInfo GetVideoInfo() => GetMediaInfo().Streams[0];
        private MediaInfo.StreamInfo GetAudioInfo()
        {
            try
            {
                MediaInfo.StreamInfo info = GetMediaInfo().Streams[1];
                return info;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public Video(string path) : base(path) { }
        public TimeSpan Duration => GetMediaInfo().Duration;
        public string VideoCodecFullName => GetVideoInfo().CodecLongName;
        public string VideoCodecName => GetVideoInfo().CodecName;
        public string PixelFormat => GetVideoInfo().PixelFormat;
        public float FrameRate => GetVideoInfo().FrameRate;
        public int Width => GetVideoInfo().Width;
        public int Height => GetVideoInfo().Height;
        public string AudioCodecFullName
        {
            get
            {
                if(GetAudioInfo() is null)
                {
                    return string.Empty;
                }
                return GetAudioInfo().CodecLongName;
            }
        }
        public string AudioCodecName
        {
            get
            {
                if (GetAudioInfo() is null)
                {
                    return string.Empty;
                }
                return GetAudioInfo().CodecName;
            }
        }
        public override string ConsoleDisplay(){
            return base.ConsoleDisplay() + $"\n\t{Width}x{Height}\n\t{Duration}\n\tVideo Codec: {VideoCodecName} ({VideoCodecFullName})" +
                $"\n\tAudio Codec: {AudioCodecName} ({AudioCodecFullName})\n\tFrameRate: {FrameRate}\n\tPixel Format: {PixelFormat}";
        }
    }
}
