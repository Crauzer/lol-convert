﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lol_convert;

[Verb("convert-champion", HelpText = "Convert a champion Wad file into a json asset blob")]
public class ConvertChampionOptions
{
    [Option('w', "wad", Required = true, HelpText = "Path to the champion Wad file")]
    public string WadPath { get; set; }

    [Option('h', "hashtable", Required = true, HelpText = "Path to the wad hasthable file to use")]
    public string HashtablePath { get; set; }

    [Option('o', "out", Required = true, HelpText = "Path to the output file")]
    public string OutputPath { get; set; }
}

[Verb("convert-league", HelpText = "Convert a league installation into a portable JSON format")]
public class ConvertLeagueOptions
{
    [Option('f', "final", Required = true, HelpText = "Path to the FINAL assets folder")]
    public string FinalPath { get; set; }

    [Option('h', "hashtable", Required = true, HelpText = "Path to the wad hasthable file to use")]
    public string HashtablePath { get; set; }

    [Option('o', "out", Required = true, HelpText = "Path to the output directory")]
    public string OutputPath { get; set; }

    [Option("convert-champions", Default = true, HelpText = "Convert champions")]
    public bool? ConvertChampions { get; set; }

    [Option("convert-champion-skins", Default = true, HelpText = "Convert champion skins")]
    public bool? ConvertChampionSkins { get; set; }
}