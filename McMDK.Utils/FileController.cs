using System;
using System.IO;

namespace McMDK.Utils
{
    public class FileController
    {
        public static void CreateDirectory(string name)
        {
            if (!Directory.Exists((name)))
            {
                Directory.CreateDirectory(name);
            }
        }

        public static void Delete(string path)
        {
            if (path.EndsWith("/") || path.EndsWith("\\"))
            {
                return;
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }
        }

        private static void DeleteDirectory(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var info in dirInfo.GetFiles())
            {
                if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    info.Attributes = FileAttributes.Normal;
                }
            }

            foreach (var info in dirInfo.GetDirectories())
            {
                DeleteDirectory(info.FullName);
            }

            if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                dirInfo.Attributes = FileAttributes.Directory;
            }
            dirInfo.Delete(true);
        }

        public static bool Exists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        public static void Copy(string source, string dest)
        {
            string dirname;
            dirname = Path.GetDirectoryName(dest);
            if (dirname == null)
            {
                throw new ArgumentException("dest");
            }
            if (File.Exists(source))
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dirname);
                }
                File.Copy(source, dest);
                return;
            }
            if (Directory.Exists(source))
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                dest += "\\";
                string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (!Directory.Exists(dirname))
                    {
                        Directory.CreateDirectory(dirname);
                    }
                    File.Copy(file, dest + file, true);
                }
            }
        }

        public static void Rename(string oldname, string newname)
        {
            File.Move(oldname, newname);
        }

        public static string[] LoadDirectory(string path, bool isDir)
        {
            if (Directory.Exists(path))
            {
                if (isDir)
                {
                    return Directory.GetDirectories(path);
                }
                return Directory.GetFiles(path);
            }
            throw new DirectoryNotFoundException();
        }

        public static string LoadFile(string path)
        {
            if (File.Exists(path))
            {
                var sr = new StreamReader(path);
                return sr.ReadToEnd();
            }
            throw new FileNotFoundException();
        }
    }
}
