using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReadMedia.Media;

namespace ReadMedia
{
    enum MediaTipo
    {
        Imagen,
        Video,
        Otro
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = string.Empty;
            while(!Directory.Exists(path)){
                Console.WriteLine("Inserte Folder");
                path = Console.ReadLine();
            }
            if(path.Last() != '\\') path = path + '\\';
            FileInfo[] fileInfos = new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.Name).ToArray();
            List<Video> videos = new List<Video>();
            List<Photo> photos = new List<Photo>();
            foreach (var item in fileInfos)
            {
                switch(Tipo(item.Extension)){
                    case MediaTipo.Imagen:
                        photos.Add(new Photo(item.FullName));
                        break;
                    case MediaTipo.Video:
                        videos.Add(new Video(item.FullName));
                        break;
                    default:
                        throw new Exception("No Media Admited");
                }
            }
            List<MyFileInfo> files = new List<MyFileInfo>();
            files.AddRange(videos); files.AddRange(photos);
            Console.WriteLine(DateTime.Now + " - Iniciar Ordenamiento");
            files.Sort(MyFileInfo.SortVoid);
            Console.WriteLine(DateTime.Now + " - Ordenamiento Terminando");
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            DateTime init = DateTime.Now;
            DateTime start = DateTime.Now;
            int count = 0;
            foreach (var item in files)
            {
                count++;
                builder = builder.Append(count + "\t" + item.ConsoleDisplay() + "\n");
                TimeSpan span = DateTime.Now - start;
                if(span.TotalMilliseconds > 995)
                {
                    Console.Write(builder.ToString());
                    builder = builder.Clear();
                    start = DateTime.Now;
                    GC.Collect();
                }
            }
            if(builder.Length > 0)
            {
                Console.Write(builder.ToString());
                builder = builder.Clear();
                GC.Collect();
            }
            Console.WriteLine("Ejecución terminada: " + DateTime.Now);
            Console.WriteLine("Duración: " + (DateTime.Now - init).TotalSeconds);
            Console.ReadLine();
        }
        private static MediaTipo Tipo(string extension){
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".png":
                case ".gif":
                    return MediaTipo.Imagen;
                case ".mp4":
                case ".mpeg4":
                case ".webm":
                    return MediaTipo.Video;
                default:
                    return MediaTipo.Otro;
            }
        }
    }
}