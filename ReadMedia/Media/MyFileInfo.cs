using System;
using System.IO;

namespace ReadMedia.Media
{
    interface IVisualProp
    {
        int Height { get; }
        int Width { get; }
    }
    enum NaturalFileSize
    {
        B = 0,
        KB = 1,
        MB = 2,
        GB = 3
    }
    public abstract class MyFileInfo : IVisualProp
    {
        protected readonly FileInfo fileInfo;
        protected MyFileInfo(string path) => fileInfo = new FileInfo(path);
        public string FullName => fileInfo.FullName;
        DirectoryInfo Directory => fileInfo.Directory;
        public string Name => fileInfo.Name;
        public string OnlyName => Name.Remove(Name.LastIndexOf('.'));
        public string Extension => fileInfo.Extension;
        public long FileSize => fileInfo.Length;
        public string NaturalSize
        {
            get
            {
                double div(NaturalFileSize a) => FileSize / Math.Pow(1024, (int)a);
                bool fitKilo(NaturalFileSize a) =>  div(a) < 1;
                string res(NaturalFileSize a) => $"{string.Format("{0:0.###}", div(a))}{a}";

                if (fitKilo(NaturalFileSize.KB)) return res(NaturalFileSize.B);
                if (fitKilo(NaturalFileSize.MB)) return res(NaturalFileSize.KB);
                if (fitKilo(NaturalFileSize.GB)) return res(NaturalFileSize.MB);
                return res(NaturalFileSize.GB);
            }
        }
        public bool IsReadOnly => fileInfo.IsReadOnly?true:false;
        public bool Exists => fileInfo.Exists;
        public DateTime Creation => fileInfo.CreationTime;
        public DateTime LastWrite => fileInfo.LastWriteTime;
        public DateTime LastAccess => fileInfo.LastAccessTime;
        public DateTime CreationUTC => fileInfo.CreationTimeUtc;
        public DateTime LastWriteUTC => fileInfo.LastWriteTimeUtc;
        public DateTime LastAccessUTC => fileInfo.LastAccessTimeUtc;
        public virtual int Height => throw new NotImplementedException();
        public virtual int Width => throw new NotImplementedException();
        public FileInfo CopyTo(string destFileName) => fileInfo.CopyTo(destFileName);
        public FileInfo CopyTo(string destFileName, bool overwrite) => fileInfo.CopyTo(destFileName,overwrite);
        public void MoveTo(string destFileName) => fileInfo.MoveTo(destFileName);
        protected static string CSVHeader()
        {
            return "Name,OnlyName,Extension,FileSize,NaturalSize,Creation,LastWrite,LastAccess,CreationUTC,LastWriteUTC,LastAccessUTC,Height,Width";
        }
        public virtual string CSVregister()
        {
            return $"{Name},{OnlyName},{Extension},{FileSize},{NaturalSize},{Creation},{LastWrite},{LastAccess},{CreationUTC},{LastWriteUTC},{LastAccessUTC},{Height},{Width}";
        }
        public virtual string ConsoleDisplay(){
            return $"{Name}\n\t{NaturalSize}\n\tCreación: {Creation} (UTC:{CreationUTC})\n\tModificado: {LastWrite} (UTC:{LastWriteUTC})\n\tAcceso: {LastAccess} (UTC:{LastAccessUTC})";
        }
        public static int SortVoid(MyFileInfo a, MyFileInfo b)
        {
            return a.LastWriteUTC.CompareTo(b.LastWriteUTC);
        }
    }
}