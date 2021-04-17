using System;
using System.IO;
using System.Linq;

namespace SmartArmenia.SmartFileComparer
{
    public static class FileCompare
    {
        public static bool CompareDirectories(string path1, string path2, bool compareFilesContent)
        {
            if (!path1.EndsWith(Path.DirectorySeparatorChar))
            {
                path1 += Path.DirectorySeparatorChar;
            }

            if (!path2.EndsWith(Path.DirectorySeparatorChar))
            {
                path2 += Path.DirectorySeparatorChar;
            }

            CheckDirectoryExists(path1);
            CheckDirectoryExists(path2);

            var files1 = Directory.GetFiles(path1, "*.*", SearchOption.AllDirectories)
                .Select(x => x.Replace(path1, "")).ToArray();
            var files2 = Directory.GetFiles(path2, "*.*", SearchOption.AllDirectories)
                .Select(x => x.Replace(path2, "")).ToArray();

            var isEqual = files1.SequenceEqual(files2, StringComparer.InvariantCultureIgnoreCase);

            if (!isEqual || !compareFilesContent) return isEqual;
            return files1.All(file => CompareFiles(Path.Combine(path1, file), Path.Combine(path2, file)));
        }

        public static bool CompareFiles(string file1, string file2)
        {
            CheckFileExists(file1);
            CheckFileExists(file2);

            if (file1 == file2)
            {
                return true;
            }

            var fs1 = new FileStream(file1, FileMode.Open);
            var fs2 = new FileStream(file2, FileMode.Open);

            try
            {
                if (fs1.Length != fs2.Length)
                {
                    return false;
                }

                int file1Byte;
                int file2Byte;
                do
                {
                    file1Byte = fs1.ReadByte();
                    file2Byte = fs2.ReadByte();
                } while (file1Byte == file2Byte && file1Byte != -1);

                return file1Byte - file2Byte == 0;
            }
            finally
            {
                fs1.Close();
                fs2.Close();
            }
        }

        private static void CheckFileExists(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception($"File {file} does not exists");
            }
        }

        private static void CheckDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception($"Directory {path} does not exists");
            }
        }
    }
}