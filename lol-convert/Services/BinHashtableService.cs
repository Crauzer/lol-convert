﻿using System.Globalization;
using LeagueToolkit.Meta;
using Serilog;

namespace lol_convert.Services;

internal static class BinHashtableService
{
    private static readonly Dictionary<uint, string> _hashes = [];
    private static readonly Dictionary<uint, string> _objectHashes = [];

    public static void LoadBinHashes(string location)
    {
        Log.Information("Loading bin hashes from {location}", location);
        LoadHashes(location, _hashes);
    }

    public static void LoadBinObjects(string location)
    {
        Log.Information("Loading bin object hashes from {location}", location);
        LoadHashes(location, _objectHashes);
    }

    private static void LoadHashes(string location, Dictionary<uint, string> hashtable)
    {
        using StreamReader reader = new(location);
        while (reader.ReadLine() is string line)
        {
            int separatorIndex = line.IndexOf(' ');
            uint hash = uint.Parse(line.AsSpan(0, separatorIndex), NumberStyles.HexNumber);

            hashtable.TryAdd(hash, line[(separatorIndex + 1)..]);
        }
    }

    public static string ResolveHash(uint hash) =>
        _hashes.GetValueOrDefault(hash, string.Format("{0:x8}", hash));

    public static string ResolveHash(MetaHash hash) => ResolveHash(hash.Hash);

    public static string ResolveObjectLink(uint hash) =>
        _objectHashes.GetValueOrDefault(hash, string.Format("{0:x8}", hash));

    public static string TryResolveHash(uint hash) => hash == 0 ? null : ResolveHash(hash);
    public static string TryResolveHash(MetaHash hash) => TryResolveHash(hash.Hash);

    public static string TryResolveObjectLink(uint hash) =>
        hash == 0 ? null : ResolveObjectLink(hash);
}
