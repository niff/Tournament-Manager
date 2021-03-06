﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Tools
{
    public static class ResultsParser
    {

        public const string ParseScorePattern =
            @"(?<level>[1-7])(?<color>s|h|c|d|nt)(?<double>x{0,2})(?<player>[nesw])(?<tricks>(=|[+-](1[0-3]|[1-9])))";

        public static readonly Dictionary<string, PlayedBy> PlayersDictionary = new Dictionary<string, PlayedBy>
        {
            {"S", PlayedBy.South},
            {"W", PlayedBy.West},
            {"N", PlayedBy.North},
            {"E", PlayedBy.East}
        };

        public static readonly Dictionary<string, ContractColors> ColorsDictionary = new Dictionary<string, ContractColors>
        {
            {"S", ContractColors.Spade},
            {"H", ContractColors.Heart},
            {"D", ContractColors.Diamond},
            {"C", ContractColors.Club},
            {"NT",ContractColors.NoTrump}
        };

         public static readonly Dictionary<string, ContractColors> ColorsHtmlDictionary = new Dictionary<string, ContractColors>
        {
            {"", ContractColors.Unknown},
            {"<i class=\"glyphicon result-icon spade\"></i>", ContractColors.Spade},
            {"<i class=\"glyphicon result-icon heart\"></i>", ContractColors.Heart},
            {"<i class=\"glyphicon result-icon diamond\"></i>", ContractColors.Diamond},
            {"<i class=\"glyphicon result-icon club\"></i>", ContractColors.Club},
            {"<strong>NT</strong>",ContractColors.NoTrump}
        };
         public static readonly Dictionary<ContractColors, string> ColorsEnumDictionary = new Dictionary<ContractColors, string>
        {
            {ContractColors.Unknown, ""},
            {ContractColors.Spade, "<i class=\"glyphicon result-icon spade\"></i>" },
            {ContractColors.Heart, "<i class=\"glyphicon result-icon heart\"></i>"},
            {ContractColors.Diamond, "<i class=\"glyphicon result-icon diamond\"></i>"},
            {ContractColors.Club, "<i class=\"glyphicon result-icon club\"></i>"},
            {ContractColors.NoTrump, "<strong>NT</strong>"}
        };

        public static readonly Dictionary<string, ContractDoubled> DoubledDictionary = new Dictionary<string, ContractDoubled>
        {
            {"", ContractDoubled.NotDoubled},
            {"x", ContractDoubled.Doubled},
            {"xx", ContractDoubled.Redoubled}
        };

         
        public static string GetFormatResult(Result result)
        {
            return GetFormatResult(result, ColorsDictionary);
        }

        public static string GetFormatResultForHtml(Result result)
        {
             return GetFormatResult(result, ColorsHtmlDictionary);
        }

         
        private static string GetFormatResult(Result result, Dictionary<string, ContractColors> colorsDictionary)
         {
             if (result.PlayedBy == PlayedBy.Unavailable)
                 return "";
             if (result.PlayedBy == PlayedBy.PassedOut)
                 return "Passed out";
             if (result.PlayedBy == PlayedBy.DirectorScore)
                 return "Director score";
             if (result.ContractColor == ContractColors.Unknown)
                return "";


             var tricks = result.NumberOfTricks - result.ContractLevel - 6;
             var tricksString = tricks == 0 ? "=" : tricks.ToString("+#;-#;0");
             var doubledString = DoubledDictionary.FirstOrDefault(kpv => kpv.Value == result.ContractDoubled).Key;
             
             return string.Format("{0}{1}{2} {3} {4}",
                 result.ContractLevel,
                 colorsDictionary.First(kpv => kpv.Value == result.ContractColor).Key,
                 doubledString,
                 PlayersDictionary.First(kpv => kpv.Value == result.PlayedBy).Key,
                 tricksString);
         }

        public static Result Parse(string stringToParse)
        {
            var r = new Regex(ParseScorePattern);
            var preparedString = String.Join("", stringToParse.Where(c => !char.IsWhiteSpace(c))).ToLowerInvariant();
            if (!r.IsMatch(preparedString))
                return null;
            Match m = r.Match(preparedString);

            var contractLevel = int.Parse(m.Groups["level"].Value);

            var result = new Result
            {
                ContractLevel = contractLevel,
                ContractColor = ParseContractColor(m.Groups["color"].Value),
                PlayedBy = ParsePlayedBy(m.Groups["player"].Value),
                ContractDoubled = ParseDoubled(m.Groups["double"].Value),
                NumberOfTricks = ParseNumberOfTricks(m.Groups["tricks"].Value, contractLevel)
            };
            return result;

        }

        public static Result UpdateResult(Result original, Result resultToCopy)
        {
            original.ContractLevel = resultToCopy.ContractLevel;
            original.ContractColor = resultToCopy.ContractColor;
            original.NumberOfTricks = resultToCopy.NumberOfTricks;
            original.ContractDoubled = resultToCopy.ContractDoubled;
            original.PlayedBy = resultToCopy.PlayedBy;

            return original;
        }

        public static bool ResultIsEntered(Result result)
        {
            return result.PlayedBy == PlayedBy.Unavailable;
        }

        private static int ParseNumberOfTricks(string numberOfTricksString, int contractLevel)
        {
            var tricks = numberOfTricksString.StartsWith("=") ? 0 : int.Parse(numberOfTricksString);
            return 6 + contractLevel + tricks;
        }

        private static ContractDoubled ParseDoubled(string doubleString)
        {
            return DoubledDictionary[doubleString.ToLowerInvariant()];
        }

        private static PlayedBy ParsePlayedBy(string playerString)
        {
            return PlayersDictionary[playerString.ToUpperInvariant()];
        }

        private static ContractColors ParseContractColor(string colorString)
        {
            return ColorsDictionary[colorString.ToUpperInvariant()];
        }


    }
}
