using System.Drawing;

namespace ReadMedia.Media
{
    public class Photo : MyFileInfo
    {
        private Image GetImage() => Image.FromFile(FullName);
        public Photo(string path) : base(path) { }
        public float VerticalResolution => GetImage().VerticalResolution;
        public float HorizontalResolution => GetImage().HorizontalResolution;
        public int Width => GetImage().Width;
        public int Height => GetImage().Height;
        public override string ConsoleDisplay(){
            return base.ConsoleDisplay() + $"\n\t{Width}x{Height}";
        }
    }
}
