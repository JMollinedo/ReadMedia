using System.Drawing;

namespace ReadMedia.Media
{
    public class Photo : MyFileInfo
    {
        private readonly int width;
        private readonly int height;
        public Photo(string path) : base(path)
        {
            Image image = Image.FromFile(FullName);
            VerticalResolution = image.VerticalResolution;
            HorizontalResolution = image.HorizontalResolution;
            width = image.Width;
            height = image.Height;
            image.Dispose();
        }
        public float VerticalResolution { private set; get; }
        public float HorizontalResolution { private set; get; }
        public override int Width => width;
        public override int Height => height;
        public override string ConsoleDisplay(){
            return base.ConsoleDisplay();
        }
        public new static string CSVHeader()
        {
            return MyFileInfo.CSVHeader() + ",VerticalResolution,HorzontalResolution";
        }
        public override string CSVregister()
        {
            return base.CSVregister() + $",{VerticalResolution},{HorizontalResolution}";
        }
    }
}
