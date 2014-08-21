using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IglaClub.TournamentManager.Tests
{
    [TestClass]
    public class Parse
    {
        [TestMethod]
        public void OneSpadeJustMadeByNorth()
        {

            var expected = CreateNewResult(1, ContractColors.Spade, 7, ContractDoubled.NotDoubled, NESW.North);

            var result = ResultsParser.Parse("1sN=");

            CompareResults(expected, result);
        }

        [TestMethod]
        public void ThreeHeartsPlusOneDoubledBySouth()
        {

            var expected = CreateNewResult(3, ContractColors.Heart, 10, ContractDoubled.Doubled, NESW.South);

            var result = ResultsParser.Parse("3h  x s +1");

            CompareResults(expected, result);
        }

        [TestMethod]
        public void ThreeHeartsDblPlusOne()
        {

            var expected = CreateNewResult(3, ContractColors.Heart, 10, ContractDoubled.Doubled, NESW.South);

            var result = ResultsParser.Parse("3hxs+1");

            CompareResults(expected, result);
        }

        [TestMethod]
        public void SevenNtDoubledMinusTwoByN()
        {

            var expected = CreateNewResult(7, ContractColors.NoTrump, 11, ContractDoubled.Doubled, NESW.North);

            var result = ResultsParser.Parse("7nt x N-2");

            CompareResults(expected, result);
        }

        [TestMethod]
        public void OneSpadeMinus0()
        {

            var result = ResultsParser.Parse("1sN-0");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void OneSpadeEqual3()
        {

            var expected = CreateNewResult(1, ContractColors.Spade, 7, ContractDoubled.NotDoubled, NESW.North);

            var result = ResultsParser.Parse("1sN=5");

            CompareResults(expected, result);
        }


        private static Result CreateNewResult(int level, ContractColors contractColors,
            int numberOfTricks, ContractDoubled contractDoubled, NESW playedBy)
        {
            var expected = new Result()
            {
                ContractLevel = level,
                ContractColor = contractColors,
                NumberOfTricks = numberOfTricks,
                ContractDoubled = contractDoubled,
                PlayedBy = playedBy
            };
            return expected;
        }

        private static void CompareResults(Result expected, Result result)
        {
            Assert.AreEqual(expected.ContractLevel, result.ContractLevel);
            Assert.AreEqual(expected.ContractColor, result.ContractColor);
            Assert.AreEqual(expected.NumberOfTricks, result.NumberOfTricks);
            Assert.AreEqual(expected.ContractDoubled, result.ContractDoubled);
            Assert.AreEqual(expected.PlayedBy,result.PlayedBy);
        }
    }

    public class ResultsParser
    {
        public const string Pattern =
            @"(?<level>[1-7])(?<color>s|h|c|d|nt)(?<double>x{0,2})(?<player>[nesw])(?<tricks>(=|[+-](1[0-3]|[1-9])))";
        public static Result Parse(string stringToParse)
        {
            var r = new Regex(Pattern);
            var preparedString = String.Join("",stringToParse.Where(c=>!char.IsWhiteSpace(c))).ToLowerInvariant();
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

        private static int ParseNumberOfTricks(string numberOfTricksString, int contractLevel)
        {
            var tricks = numberOfTricksString.StartsWith("=") ? 0 : int.Parse(numberOfTricksString); 
            return 6 + contractLevel + tricks;
        }

        private static ContractDoubled ParseDoubled(string doubleString)
        {
            switch (doubleString)
            {
                case "":
                    return ContractDoubled.NotDoubled;
                case "x":
                    return ContractDoubled.Doubled;
                case "xx":
                    return ContractDoubled.Redoubled;
                default:
                    return ContractDoubled.NotDoubled;
            }
        }

        private static NESW ParsePlayedBy(string playerString)
        {
            switch (playerString)
            {
                case "s":
                    return NESW.South;
                case "n":
                    return NESW.North;
                case "e":
                    return NESW.East;
                case "w":
                    return NESW.West;
                default:
                    return NESW.Unavailable;
            }
        }

        private static ContractColors ParseContractColor(string colorString)
        {
            switch (colorString)
            {
                case "s":
                    return ContractColors.Spade;
                case "h":
                    return ContractColors.Heart;
                case "c":
                    return ContractColors.Club;
                case "d":
                    return ContractColors.Diamond;
                default:
                    return ContractColors.NoTrump;
            }
        }
    }
}
