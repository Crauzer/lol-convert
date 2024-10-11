namespace lol_convert.Utils;

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

    public static string FileNameWithoutMultiExtension(string path)
    {
        path = Path.GetFileNameWithoutExtension(path);
        return path.Remove(path.IndexOf('.'));
    }
}
