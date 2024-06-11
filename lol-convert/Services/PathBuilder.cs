namespace lol_convert.Services;

internal static class PathBuilder
{
    public static DirectoryInfo CreateMapSkinAssetDirectory(string mapName, string skinName) =>
        Directory.CreateDirectory(Path.Combine("assets", "maps", mapName, "skins", skinName));

    public static DirectoryInfo CreateMapSkinDataDirectory(string mapName, string skinName) =>
        Directory.CreateDirectory(Path.Combine("data", "maps", mapName, "skins", skinName));

    public static string GenerateMapSkinAssetDirectoryPath(string mapName, string skinName) =>
        Path.Combine("assets", "maps", mapName, "skins", skinName);

    public static string GenerateMapSkinAssetPath(string mapName, string skinName) =>
        Path.Combine(GenerateMapSkinAssetDirectoryPath(mapName, skinName), $"{skinName}.glb");

    public static string GenerateMapSkinDataDirectoryPath(string mapName, string skinName) =>
        Path.Combine("data", "maps", mapName, "skins", skinName);

    public static string GenerateMapSkinPackagePath(string mapName, string skinName) =>
        Path.Combine(GenerateMapSkinDataDirectoryPath(mapName, skinName), $"{skinName}.json");
}
