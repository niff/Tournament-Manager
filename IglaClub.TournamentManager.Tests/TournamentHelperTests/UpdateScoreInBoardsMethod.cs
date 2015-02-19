using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using IglaClub.TournamentManager;

namespace IglaClub.TournamentManager.TournamentHelperTests.Tests
{
    [TestClass]
    public class UpdateScoreInBoardsMethod
    {
        [TestMethod]
        public void CalculateSimpleNonVulScore()
        {
            var result = CreateNewResult(3, ContractColors.Club, 8, ContractDoubled.NotDoubled, NESW.South);

            var score = TournamentHelper.CalculateScoreInBoard(result, false);

            Assert.AreEqual(-50, score);
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
    }
}
