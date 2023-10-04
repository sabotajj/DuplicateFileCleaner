using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IWshRuntimeLibrary;

namespace shortcutCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fileTypeList = new string[] { "jpg", "jpeg", "mov", "mpg", "mp4", "avi" };
            var AllFilesInSourceDir = getAllFilesListWithPath(args[0]);
            var AllFilesInDestDir = getAllFilesListWithPath(args[1]);
            int i = 0;
            foreach(var sourceFile in AllFilesInSourceDir)
            {
                Console.WriteLine(++i+"/"+AllFilesInSourceDir.Count);
                if (!fileTypeList.Any(ext => "."+ext == Path.GetExtension(sourceFile).ToLower())) continue;
                string fileName = Path.GetFileName(sourceFile);
                var fileFullPath = findFile(fileName, AllFilesInDestDir) ;
                if (fileFullPath == null) continue;
                else
                {
                    createShortcut(fileName,
                        Path.GetDirectoryName(sourceFile),
                        fileFullPath
                    );
                    System.IO.File.Delete(sourceFile);
                }
            }
        }
        private static string findFile(string fileName, List<string> fileList)
        {
            return fileList.Where(file => Path.GetFileName(file).ToLower() == fileName.ToLower()).FirstOrDefault();
        }
        private static List<string> getAllFilesListWithPath(string rootPath)
        {
            var filesList = new List<string>();
            filesList.AddRange(
                getFilesInDir(rootPath).ToList()
            );
            // if there are still directories
            var subDirs = getFolderListInDir(rootPath);
            foreach(string subDir in subDirs)
            {
                filesList.AddRange(getAllFilesListWithPath(subDir));
            }
            return filesList;
        }
        private static string[] getFolderListInDir(string filePath)
        {
            var currentDirs = Directory.GetDirectories(filePath);
            return currentDirs;
        }
        private static void createShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = targetFileLocation;   // The description of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
        private static string[] getFilesInDir(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);   
        }
        
    }
}
