using System;
using System.IO;

namespace shortcutCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fileTypeList = new string[] { "jpg", "jpeg", "mov", "mpg", "mp4", "avi" };
            var AllFilesInSourceDir = FileUtilities.GetAllFilesPathRecursive(args[0]);
            var AllFilesInDestDir = FileUtilities.GetAllFilesPathRecursive(args[1]);
            int i = 0;
            foreach (var sourceFile in AllFilesInSourceDir)
            {
                Console.WriteLine($@"Working on file {sourceFile} ({i} / {AllFilesInSourceDir.Count})");

                if (!FileUtilities.IsFileSupported(fileTypeList, sourceFile))
                    continue;

                string fileName = Path.GetFileName(sourceFile);
                var filePathInDestination = FileUtilities.GetFileFullPath(fileName, AllFilesInDestDir);
                
                if (filePathInDestination == null) continue;

                FileUtilities.CreateShortcut(fileName,
                    Path.GetDirectoryName(sourceFile),
                    filePathInDestination
                );

                Console.WriteLine($@"Deleting file {sourceFile}");
                try
                {
                    File.Delete(sourceFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Error while deleting file {sourceFile}. Details : {ex.Message}");
                }
            }
        }
        
        
    }
}
