namespace lol_convert;

internal static class FsUtils
{
    public static void ClearDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }
        foreach (var file in Directory.EnumerateFiles(path))
        {
            File.Delete(file);
        }
        foreach (var dir in Directory.EnumerateDirectories(path))
        {
            Directory.Delete(dir, true);
        }
    }
}
