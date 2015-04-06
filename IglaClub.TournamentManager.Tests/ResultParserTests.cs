using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using IglaClub.ObjectModel.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IglaClub.TournamentManager.Tests
{
    [TestClass]
    public class ResultsParserTests
    {
        [TestClass]
        public class ParseMethod
        {
            [TestMethod]
            public void OneSpadeJustMadeByNorth()
            {
                var expected = CreateNewResult(1, ContractColors.Spade, 7, ContractDoubled.NotDoubled, PlayedBy.North);

                var result = ResultsParser.Parse("1sN=");

                CompareResults(expected, result);
            }

            [TestMethod]
            public void ThreeHeartsPlusOneDoubledBySouth()
            {
                var expected = CreateNewResult(3, ContractColors.Heart, 10, ContractDoubled.Doubled, PlayedBy.South);

                var result = ResultsParser.Parse("3h  x s +1");

                CompareResults(expected, result);
            }

            [TestMethod]
            public void ThreeHeartsDblPlusOne()
            {
                var expected = CreateNewResult(3, ContractColors.Heart, 10, ContractDoubled.Doubled, PlayedBy.South);

                var result = ResultsParser.Parse("3hxs+1");

                CompareResults(expected, result);
            }

            [TestMethod]
            public void SevenNtDoubledMinusTwoByN()
            {
                var expected = CreateNewResult(7, ContractColors.NoTrump, 11, ContractDoubled.Doubled, PlayedBy.North);

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
                var expected = CreateNewResult(1, ContractColors.Spade, 7, ContractDoubled.NotDoubled, PlayedBy.North);

                var result = ResultsParser.Parse("1sN=3");

                CompareResults(expected, result);
            }

            [TestMethod]
            public void TwoClubsRedoubledPlusOne()
            {
                var expected = CreateNewResult(2, ContractColors.Club, 9, ContractDoubled.Redoubled, PlayedBy.North);

                var result = ResultsParser.Parse("2cxxN+1");

                CompareResults(expected, result);
            }

            [TestMethod]
            public void TricksEnteredWithoutDash()
            {
                var expected = CreateNewResult(5, ContractColors.Club, 6, ContractDoubled.Redoubled, PlayedBy.East);

                var result = ResultsParser.Parse("5 c xx E 6");

                CompareResults(expected, result);
            }
            
        }

        [TestClass]
        public class FormatResultMethod
        {
            [TestMethod]
            public void ThreeHeartsByEastPlusOne()
            {
                var data = CreateNewResult(3, ContractColors.Heart, 10, ContractDoubled.Doubled, PlayedBy.East);

                var result = ResultsParser.GetFormatResult(data);

                Assert.AreEqual("3Hx E +1", result);
            }

            [TestMethod]
            public void OneSpadeByNorthJustMade()
            {
                var data = CreateNewResult(1, ContractColors.Spade, 7, ContractDoubled.NotDoubled, PlayedBy.North);

                var result = ResultsParser.GetFormatResult(data);

                Assert.AreEqual("1S N =", result);
            }

            [TestMethod]
            public void SevenNoTrumphDoubledMinus2ByS()
            {
                var data = CreateNewResult(7, ContractColors.NoTrump, 11, ContractDoubled.Doubled, PlayedBy.South);

                var result = ResultsParser.GetFormatResult(data);

                Assert.AreEqual("7NTx S -2", result);
            }

            [TestMethod]
            public void PassedOut()
            {
                var data = CreateNewResult(7, ContractColors.NoTrump, 11, ContractDoubled.Doubled, PlayedBy.PassedOut);

                var result = ResultsParser.GetFormatResult(data);

                Assert.AreEqual("Passed out", result);
            }

            [TestMethod]
            public void DirectorScore()
            {
                var data = CreateNewResult(7, ContractColors.NoTrump, 11, ContractDoubled.Doubled, PlayedBy.DirectorScore);

                var result = ResultsParser.GetFormatResult(data);

                Assert.AreEqual("Director score", result);
            }
        }

        private static Result CreateNewResult(int level, ContractColors contractColors,
            int numberOfTricks, ContractDoubled contractDoubled, PlayedBy playedBy)
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
            Assert.AreEqual(expected.PlayedBy, result.PlayedBy);
        }
    }

   
}
