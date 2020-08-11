using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static MediaTipo Tipo(string extension)
        {
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
        private static void Files(IEnumerable<MyFileInfo> files, string header, string path, ref int count)
        {
            FileManager.Write(path, "id," + header + "\n", true);
            int a = 0;
            foreach (var item in files)
            {
                a++;
                FileManager.Write(path, $"{a},{item.CSVregister()}\n", false);
                count++;
            }
            Console.WriteLine(path + " Done");
        } 
        static void Main(string[] args)
        {
            string path = string.Empty;
            while(!FileManager.FolderExists(path)){
                Console.WriteLine("Insert Folder");
                path = Console.ReadLine();
            }
            if(path.Last() != '\\') path = path + '\\';
            Console.WriteLine(DateTime.Now + "\tReading " + path);
            var videos = new List<Video>(); var photos = new List<Photo>();
            int count = 0;
            DateTime start = DateTime.Now;
            TimeSpan span;
            FFFProbeProvider provider = new FFFProbeProvider();
            foreach (var item in FileManager.GetFiles(path))
            {
                switch(Tipo(FileManager.Extention(item))){
                    case MediaTipo.Imagen:
                        photos.Add(new Photo(item));
                        break;
                    case MediaTipo.Video:
                        videos.Add(new Video(item,provider.Val));
                        break;
                    default:
                        throw new Exception("No Media Admited");
                }
                count++;
                span = DateTime.Now - start;
                if (span.TotalMilliseconds > 9940)
                {
                    start = DateTime.Now;
                    Console.WriteLine($"{start}\t{count} Files Loaded");
                }
            }
            Console.WriteLine($"{DateTime.Now}\t{count} Files Loaded. Reading Finished");
            Console.WriteLine($"{DateTime.Now}\tStart Sorting");
            videos.Sort(MyFileInfo.SortVoid); photos.Sort(MyFileInfo.SortVoid);
            Console.WriteLine($"{DateTime.Now}\tSorting Finished");
            string respath = string.Empty;
            while (!FileManager.FolderExists(respath))
            {
                Console.WriteLine("Insert Output Folder");
                respath = Console.ReadLine();
            }
            if (respath.Last() != '\\') respath = respath + '\\';
            int pc = 0; int vc = 0; DateTime init = DateTime.Now;
            Thread threadV = new Thread(() => Files(videos, Video.CSVHeader(), respath + "VideosInfo.csv", ref pc));
            Thread threadF = new Thread(() => Files(photos, Photo.CSVHeader(), respath + "PhotosInfo.csv", ref vc));
            Console.WriteLine("Start Writing: " + init);
            threadF.Start(); threadV.Start();
            start = DateTime.Now;
            int done = pc + vc;
            while(done < count)
            {
                span = DateTime.Now - start;
                done = pc + vc;
                if (span.TotalMilliseconds > 930)
                {
                    start = DateTime.Now;
                    Console.WriteLine($"{start}\t{done}/{count}");
                }
            }
            Console.WriteLine("Writing Finished: " + DateTime.Now);
            Console.ReadLine();
        }
    }
}