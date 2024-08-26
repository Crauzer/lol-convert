namespace lol_convert.Services;

internal static class PathBuilder
{
    // -------- Character --------

    public static string CreateCharacterDataDirectoryPath(string characterName) =>
        Path.Combine("data", "characters", characterName);

    public static string CreateCharacterDataPath(string characterName) =>
        Path.Combine(CreateCharacterDataDirectoryPath(characterName), $"{characterName}.json");

    public static string CreateCharacterSkinDataDirectoryPath(
        string characterName,
        string skinName
    ) => Path.Combine(CreateCharacterDataDirectoryPath(characterName), "skins", skinName);

    public static string CreateCharacterSkinDataPath(string characterName, string skinName) =>
        Path.Combine(
            CreateCharacterSkinDataDirectoryPath(characterName, skinName),
            $"{skinName}.json"
        );

    // -------- Map --------

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
