using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace lol_convert;

[Verb("convert-league", HelpText = "Convert a league installation into a portable JSON format")]
public class ConvertLeagueOptions
{
    [Option('f', "final", Required = true, HelpText = "Path to the FINAL assets folder")]
    public string FinalPath { get; set; }

    [Option("wad-hashtable", Required = true, HelpText = "Path to the wad hasthable file to use")]
    public string WadHashtablePath { get; set; }

    [Option("bin-hashes", Required = true, HelpText = "Path to the bin hashes file to use")]
    public string BinHashesPath { get; set; }

    [Option("bin-objects", Required = true, HelpText = "Path to the bin object hashes file to use")]
    public string BinObjectHashesPath { get; set; }

    [Option('o', "out", Required = true, HelpText = "Path to the output directory")]
    public string OutputPath { get; set; }

    [Option("convert-champions", Default = true, HelpText = "Convert champions")]
    public bool? ConvertChampions { get; set; }

    [Option("convert-champion-skins", Default = true, HelpText = "Convert champion skins")]
    public bool? ConvertChampionSkins { get; set; }
}

[Verb("convert-champion", HelpText = "Convert a champion into a portable JSON format")]
public class ConvertChampionOptions
{
    [Option('f', "final", Required = true, HelpText = "Path to the FINAL assets folder")]
    public string FinalPath { get; set; }

    [Option('c', "champion", Required = true, HelpText = "Name of the champion to convert")]
    public string ChampionName { get; set; }

    [Option('o', "out", Required = true, HelpText = "Path to the output directory")]
    public string OutputPath { get; set; }

    [Option("wad-hashtable", Required = true, HelpText = "Path to the wad hasthable file to use")]
    public string WadHashtablePath { get; set; }

    [Option("bin-hashes", Required = true, HelpText = "Path to the bin hashes file to use")]
    public string BinHashesPath { get; set; }

    [Option("bin-objects", Required = true, HelpText = "Path to the bin object hashes file to use")]
    public string BinObjectHashesPath { get; set; }
}

[Verb(
    "convert-map",
    HelpText = "Convert a map into a portable JSON format, along with all of its assets"
)]
public class ConvertMapOptions
{
    [Option('f', "final", Required = true, HelpText = "Path to the FINAL assets folder")]
    public string FinalPath { get; set; }

    [Option('m', "map", Required = true, HelpText = "Name of the map to convert")]
    public string MapName { get; set; }

    [Option('o', "out", Required = true, HelpText = "Path to the output directory")]
    public string OutputPath { get; set; }

    [Option("wad-hashtable", Required = true, HelpText = "Path to the wad hasthable file to use")]
    public string WadHashtablePath { get; set; }

    [Option("bin-hashes", Required = true, HelpText = "Path to the bin hashes file to use")]
    public string BinHashesPath { get; set; }

    [Option("bin-objects", Required = true, HelpText = "Path to the bin object hashes file to use")]
    public string BinObjectHashesPath { get; set; }
}
