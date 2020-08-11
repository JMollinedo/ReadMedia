using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReadMedia.Media
{
    public static class FileManager
    {
        public static IEnumerable<string> GetFiles(string folderPath) => new DirectoryInfo(folderPath).GetFiles().Select(f => f.FullName);
        public static bool FolderExists(string folderPath) => Directory.Exists(folderPath);
        public static string Extention(string path) => new FileInfo(path).Extension;
        public static void Write(string path, string content, bool overwrite)
        {
            if (File.Exists(path) && overwrite) File.Delete(path);
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteAsync(content);
            }
        }
    }
}
