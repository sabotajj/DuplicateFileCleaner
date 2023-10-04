using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;


namespace shortcutCreator
{
    public class FileUtilities
    {
        public static bool IsFileSupported(string[] supportedFileTypes, string fileName)
        {
            return supportedFileTypes.Any(ext => "." + ext == Path.GetExtension(fileName).ToLower());
        }
        public static string GetFileFullPath(string fileName, List<string> fileList)
        {
            return fileList.FirstOrDefault(file => Path.GetFileName(file).ToLower() == fileName.ToLower());
        }
        public static List<string> GetAllFilesPathRecursive(string rootPath)
        {
            var filesList = new List<string>();
            filesList.AddRange(
                GetFilesList(rootPath).ToList()
            );
            // if there are still directories
            var subDirs = GetFolderList(rootPath);
            foreach (string subDir in subDirs)
            {
                filesList.AddRange(GetAllFilesPathRecursive(subDir));
            }
            return filesList;
        }
        public static string[] GetFolderList(string filePath)
        {
            var currentDirs = Directory.GetDirectories(filePath);
            return currentDirs;
        }
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = targetFileLocation;   // The description of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
        public static string[] GetFilesList(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }
    }
}
