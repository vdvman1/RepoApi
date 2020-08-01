using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RepositoryApi.Helpers
{
    public static class FileSystemHelpers
    {
        public const FileAttributes NotReadonly = ~(FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.Archive);

        public static void Delete(FileInfo file)
        {
            if (file.Exists)
            {
                ActionHelpers.RetryOnException(() =>
                {
                    file.Attributes &= NotReadonly;
                    file.Delete();
                });
            }
        }

        public static void Delete(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                Empty(dir);
                ActionHelpers.RetryOnException(() =>
                {
                    dir.Attributes &= NotReadonly;
                    dir.Delete();
                });
            }
        }

        public static void Empty(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.EnumerateFiles())
                {
                    Delete(file);
                }

                foreach (DirectoryInfo subDir in dir.EnumerateDirectories())
                {
                    Delete(subDir);
                }
            }
        }

        public static void Empty(string path) => Empty(new DirectoryInfo(path));

        public static void Delete(string path)
        {
            var file = new FileInfo(path);
            if(file.Exists)
            {
                Delete(file);
            }
            else
            {
                var dir = new DirectoryInfo(path);
                if(dir.Exists)
                {
                    Delete(dir);
                }
            }
        }
    }
}
