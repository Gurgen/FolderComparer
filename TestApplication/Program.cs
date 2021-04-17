using System;
using SmartArmenia.SmartFileComparer;

namespace SmartArmenia.TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(FileCompare.CompareDirectories("PATH1", "PATH2", true));

            Console.WriteLine(FileCompare.CompareFiles("FILENAME1", "FILENAME2"));
        }
    }
}