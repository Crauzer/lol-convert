﻿using System.Reflection;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using lol_convert.Packages;
using lol_convert.Wad;
using Serilog;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

public sealed class LeagueConverter
{
    public string OutputPath { get; private set; }
    public LeagueConverterOptions Options { get; private set; }

    private readonly ChampionConverter _championConverter;
    private readonly CharacterConverter _characterConverter;
    private readonly MapConverter _mapConverter;

    private readonly WadHashtable _hashtable;
    private readonly MetaEnvironment _metaEnvironment;

    public LeagueConverter(
        string outputPath,
        WadHashtable hashtable,
        LeagueConverterOptions options
    )
    {
        this.OutputPath = outputPath;
        this.Options = options;

        this._hashtable = hashtable;
        this._metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );

        this._championConverter = new(hashtable, _metaEnvironment, outputPath);
        this._characterConverter = new(hashtable, _metaEnvironment, outputPath);
        this._mapConverter = new(hashtable, _metaEnvironment, outputPath);
    }

    public LeaguePackage CreateLeaguePackage(string finalPath)
    {
        var championNames = GetChampionsList(finalPath).Order().ToList();
        var maps = _mapConverter.CreateMapPackages(finalPath);
        var _ = _championConverter.CreateChampionPackages(finalPath);
        var tftSets = CreateTftSetsData(finalPath);

        return new()
        {
            Maps = maps,
            TftSets = tftSets,
            Champions = championNames
        };
    }

    private List<string> CreateTftSetsData(string finalPath)
    {
        var tftSetPaths = ConvertUtils.GlobTftSets(finalPath).ToList();
        List<string> tftSets = [];
        foreach (var tftSetPath in tftSetPaths)
        {
            var tftSetName = Path.GetFileName(tftSetPath);

            try
            {
                CreateTftSetData(tftSetPath, finalPath);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to convert TFT set ({tftSet})", tftSetName);
            }

            tftSets.Add(tftSetName);
        }

        return tftSets;
    }

    private void CreateTftSetData(string tftSetPath, string finalPath)
    {
        using var wad = new WadFile(File.OpenRead(tftSetPath));

        var chunkPaths = ConvertUtils.ResolveWadChunkPaths(wad, _hashtable).ToList();
        var characterPaths = ConvertUtils.GlobTftCharacters(chunkPaths).ToList();
        List<string> convertedCharacterNames = [];
        foreach (var characterPath in characterPaths)
        {
            string characterName = Path.GetFileName(characterPath);
            try
            {
                var characterData = this._characterConverter.CreateCharacterPackage(
                    characterName,
                    wad,
                    chunkPaths
                );
                var _ = CharacterConverter.SaveCharacterPackage(characterData);

                convertedCharacterNames.Add(characterName);
            }
            catch (Exception exception)
            {
                Log.Error(
                    exception,
                    "Failed to convert character ({tftSet} | {characterName})",
                    Path.GetFileName(tftSetPath),
                    characterName
                );
            }
        }

        var tftSetData = new TftSet() { Characters = convertedCharacterNames };
    }

    private List<string> GetChampionsList(string finalPath)
    {
        List<string> championNames = [];

        try
        {
            var globalWad = new WadFile(Path.Join(finalPath, "Global.wad.client"));
            using var championsBinStream = globalWad.LoadChunkDecompressed(
                "global/champions/champions.bin"
            );
            var championsBin = new BinTree(championsBinStream.AsStream());

            var championHash = Fnv1a.HashLower(nameof(MetaClass.Champion));
            var championObjects = championsBin.Objects.Values.Where(x =>
                x.ClassHash == championHash
            );
            foreach (var championObject in championObjects)
            {
                var champion = MetaSerializer.Deserialize<MetaClass.Champion>(
                    this._metaEnvironment,
                    championObject
                );

                championNames.Add(champion.Name);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to get champions list");
        }

        return championNames;
    }
}

public sealed class LeagueConverterOptions
{
    public bool ConvertChampions { get; set; } = true;
    public bool ConvertChampionSkins { get; set; } = true;
}
