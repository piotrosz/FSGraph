using System;
using System.IO;
using System.Linq;

public static class FileExtensions
{
    public static bool IsHidden(this FileInfo fileInfo)
    {
        return (fileInfo.Attributes & FileAttributes.Hidden) == 0;
    }

    public static bool IsHidden(this DirectoryInfo dirInfo)
    {
        return (dirInfo.Attributes & FileAttributes.Hidden) == 0;
    }

    public static long GetSize(this DirectoryInfo dirInfo)
    {
        long dirsSize = dirInfo.GetDirectories("*", SearchOption.AllDirectories).Sum(d => d.GetFilesSize());
        long filesSize = dirInfo.GetFilesSize();

        return filesSize + dirsSize;
    }

    public static long GetFilesSize(this DirectoryInfo dirInfo)
    {
        return dirInfo.GetFiles("*.*").Sum(f => f.Length);
    }

    private static string[] sizes = { "B", "KB", "MB", "GB" };

    public static string GetFriendlySize(this DirectoryInfo dirInfo)
    {
        int order = 0;
        long length = dirInfo.GetSize();
        while (length >= 1024 && order + 1 < sizes.Length)
        {
            order++;
            length /= 1024;
        }
        return string.Format("{0:0.##} {1}", length, sizes[order]);
    }

    public static string GetFriendlySize(this FileInfo fileInfo)
    {   
        int order = 0;
        long length = fileInfo.Length;
        while (length >= 1024 && order + 1 < sizes.Length)
        {
            order++;
            length /= 1024;
        }
        return string.Format("{0:0.##} {1}", length, sizes[order]);
    }
}

