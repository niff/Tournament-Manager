using System;
using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using System.Linq;

namespace IglaClub.TournamentManager
{
    public static class TournamentHelper
    {
        public static void AddResultsToTournament(Tournament tournament, IEnumerable<Result> results)
        {
            foreach (var result in results)
            {
                tournament.Results.Add(result);
            }
        }

        public static List<Result> GenerateInitialSittingPosition(Tournament tournament)
        {
            CorrectPairNumbers(tournament.Pairs);
            List<Result> results = CreateNewResults(tournament,tournament.Pairs);
            return results;
        }

        private static void CorrectPairNumbers(IList<Pair> pairs)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                pairs[i].PairNumber = i + 1;
            }
        }

        public static void AddBoardsToTournament(Tournament tournament, IEnumerable<BoardInstance> boards)
        {
            foreach (var boardInstance in boards)
            {
                tournament.Boards.Add(boardInstance);
            }
        }

        public static IEnumerable<Result> GenerateNextCavendishRound(Tournament tournament, bool withPairRepeats)
        {
            UpdatePointsPerBoard(tournament);
            UpdatePairsResultsInTournament(tournament);
            IList<Pair> sortedPairsList = tournament.Pairs.OrderByDescending(p => p.Score).ToList();
            if (!withPairRepeats)
                CorrectRepeats(sortedPairsList, tournament.Results);
            IEnumerable<Result> newResults = CreateNewResults(tournament, sortedPairsList);
            return newResults;
        }

        private static void CorrectRepeats(IList<Pair> pairsList, IList<Result> results)
        {
            //throw new NotImplementedException();
        }

        public static void UpdatePairsResultsInTournament(Tournament tournament)
        {
            foreach (var pair in tournament.Pairs)
            {
                long pairId = pair.Id;
                float totalScore = 0;
                var results = tournament.Results.Where(r => r.EW.Id == pairId || r.NS.Id == pairId);
                totalScore = results.Sum(r => r.NS.Id == pairId ? r.ScoreNs : r.ScoreEw) ?? 0;
                pair.Score = totalScore;
            }
            
        }

        private static List<Result> CreateNewResults(Tournament tournament, IList<Pair> sortedPairsList)
        {
            var results = new List<Result>();
            if (sortedPairsList.Count % 2 == 1)
                sortedPairsList.Add(new Pair { Player1 = null, Player2 = null, PairNumber = tournament.Pairs.Max(p=>p.PairNumber) + 1 });

            for (int i = 0; i < sortedPairsList.Count / 2; i++)
            {

                for (int j = tournament.CurrentRound * tournament.BoardsInRound; j < (tournament.CurrentRound+1)*tournament.BoardsInRound; j++)
                {
                    var result = new Result
                    {
                        Tournament = tournament,
                        Board = tournament.Boards[j],
                        NS = sortedPairsList[i],
                        EW = sortedPairsList[i + sortedPairsList.Count / 2],
                        RoundNumber = tournament.CurrentRound+1,
                        TableNumber = i + 1,
                        PlayedBy = NESW.Unavailable
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        public static void UpdatePointsPerBoard(Tournament tournament)
        {
            var resultsGroupedByBoards = tournament.Results
                .Where(r=>r.ResultNsPoints != null)
                .GroupBy(r => r.Board.BoardNumber, r=>r, (key,g) => new { BoardId = key, Results = g.ToList()})
                .ToList();
            foreach (var resultsGroupedByBoard in resultsGroupedByBoards)
            {
                UpdateBoardScores(resultsGroupedByBoard.Results,tournament.TournamentScoringType);
            }
        }

        public static void UpdateBoardScores(List<Result> resultsGroupedByBoard,TournamentScoringType tournamentScoringType)
        {
            if(tournamentScoringType == TournamentScoringType.Matchpoints)
                CalculateScoresAsMatchpoints(resultsGroupedByBoard);
            else
                throw new NotSupportedException("By now only Cavendish tournament type is supported");
        }

        public static void UpdateEachResultScore(IList<Result> resultsGroupedByBoard)
        {
            foreach (var result in resultsGroupedByBoard)
            {
                int? newScore = UpdateScoreInBoards(result, DealerIsVulnerable(result));
                if (newScore == null) //that means the score was wrtitten witout the contract
                    continue;
                result.ResultNsPoints = newScore;
            }
        }

        private static void CalculateScoresAsMatchpoints(List<Result> resultsGroupedByBoard)
        {
            List<IGrouping<int?, Result>> orderedResults = resultsGroupedByBoard.GroupBy(result => result.ResultNsPoints).OrderByDescending(r => r.Key).ToList();

            var total = (resultsGroupedByBoard.Count - 1) * 2;
            var currentMax = total;

            foreach (IGrouping<int?, Result> resultsWithTheSameScore in orderedResults)
            {
                int score = currentMax - resultsWithTheSameScore.Count() + 1;
                foreach (var result in resultsWithTheSameScore)
                {
                    result.ScoreNs = score;
                    result.ScoreEw = total - score;
                }
                currentMax -= resultsWithTheSameScore.Count() * 2;
            }
        }

        public static int? UpdateScoreInBoards(Result result, bool dealerIsVulnerable)
        {
            if (result.ContractLevel == 0)
            {
                if (result.PlayedBy == NESW.PassedOut)
                    return 0;
                else 
                    return null;
            }
            int score = 0;
            int doubled = result.ContractDoubled == ContractDoubled.NotDoubled ? 1 : (result.ContractDoubled == ContractDoubled.Doubled ? 2 : 4);
            if(result.ContractLevel==0)
                return 0;
            if (result.NumberOfTricks >= result.ContractLevel + 6)
            {
                int pointsPerTrick = GetPointsPerTrick(result.ContractColor);
                score = pointsPerTrick*(result.NumberOfTricks - 6)*doubled;
                int scoreForBonus = pointsPerTrick * result.ContractLevel * doubled;
                if (result.ContractColor == ContractColors.NoTrump)
                {
                    score += 10;
                    scoreForBonus += 10;
                }
                if (scoreForBonus >= 100)
                {
                    if (dealerIsVulnerable)
                    {
                        score += 500;
                        if (result.ContractLevel == 6)
                            score += 750;
                        else if (result.ContractLevel == 7)
                            score += 1500;
                    }
                    else
                    {
                        score += 300;
                        if (result.ContractLevel == 6)
                            score += 500;
                        else if (result.ContractLevel == 7)
                            score += 1000;
                    }
                }
                else
                {
                    score += 50;
                }
                if (result.ContractDoubled == ContractDoubled.Doubled)
                    score += 50;
                else if (result.ContractDoubled == ContractDoubled.Redoubled)
                    score += 100;
            }
            else
            {
                int numberOfUndertricks = result.ContractLevel + 6 - result.NumberOfTricks;
                if (dealerIsVulnerable)
                {
                    if (doubled == 1)
                        score -= (numberOfUndertricks*100);
                    else
                    {
                        score -= ((numberOfUndertricks-1) * 100 + 50) * doubled;
                        if (numberOfUndertricks > 3)
                            score -= ((numberOfUndertricks - 3)*doubled*50);
                    }

                }
                else
                {
                    if (doubled == 1)
                        score -= (numberOfUndertricks * 50);
                    else
                        score -= ( (numberOfUndertricks -1) * 100  + 50) * doubled;
                }
                    
            }

            if (result.PlayedBy == NESW.East || result.PlayedBy == NESW.West)
                score *= -1;
            return score;
        }

        private static bool DealerIsVulnerable(Result result)
        {
            return result.Board.BoardDefinition.Vulnerability == Vulnerable.Both || 
                   (result.Board.BoardDefinition.Vulnerability ==Vulnerable.NS && (result.PlayedBy == NESW.North || result.PlayedBy == NESW.South))  ||
                   (result.Board.BoardDefinition.Vulnerability ==Vulnerable.EW && (result.PlayedBy == NESW.East || result.PlayedBy == NESW.West));
        }

        private static int GetPointsPerTrick(ContractColors contractColor)
        {
            if (contractColor == ContractColors.Club || contractColor == ContractColors.Diamond)
                return 20;
            else
                return 30;
        }

        public static NESW GetDealerByBoardNumber(int boardNumber)
        {
            switch (boardNumber % 4)
            {
                case 1:
                    return NESW.North;
                case 2:
                    return NESW.East;
                case 3:
                    return NESW.South;
                case 0:
                    return NESW.West;
                default:
                    return NESW.North;
            }
        }

        public static Vulnerable GetVulneralbityByBoardNumber(int boardNumber)
        {
            switch (boardNumber % 16)
            {
                case 1:
                    return Vulnerable.None;
                case 2:
                    return Vulnerable.NS;
                case 3:
                    return Vulnerable.EW;
                case 4:
                    return Vulnerable.Both;
                case 5:
                    return Vulnerable.NS;
                case 6:
                    return Vulnerable.EW;
                case 7:
                    return Vulnerable.Both;
                case 8:
                    return Vulnerable.None;
                case 9:
                    return Vulnerable.EW;
                case 10:
                    return Vulnerable.NS;
                case 11:
                    return Vulnerable.Both;
                case 12:
                    return Vulnerable.None;
                case 13:
                    return Vulnerable.Both;
                case 14:
                    return Vulnerable.None;
                case 15:
                    return Vulnerable.NS;
                case 0:
                    return Vulnerable.EW;
                default:
                    return Vulnerable.None;
            }
        }

    }
}
