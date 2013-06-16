using System;
using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using System.Linq;

namespace IglaClub.TournamentManager
{
    public static class TournamentHelper
    {
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

        public static void AddResultsToTournament(Tournament tournament, IEnumerable<Result> results)
        {
            foreach (var result in results)
            {
                tournament.Results.Add(result);
            }
        }

        public static IEnumerable<Result> GenerateInitialSittingPosition(Tournament tournament)
        {
            var results = new List<Result>();
            CorrectPairNumbers(tournament.Pairs);
            if (tournament.Pairs.Count % 2 == 1)
                tournament.Pairs.Add(new Pair { Player1 = null, Player2 = null, PairNumber = tournament.Pairs.Count + 1 });

            for (int i = 0; i < tournament.Pairs.Count / 2; i++)
            {
                for (int j = 0; j < tournament.BoardsInRound; j++)
                {
                    var result = new Result
                        {
                        Tournament = tournament,
                        Board = tournament.Boards[j],
                        NS = tournament.Pairs[i],
                        EW = tournament.Pairs[i + tournament.Pairs.Count / 2],
                        RoundNumber = 1,
                        TableNumber = i + 1
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        public static void CorrectPairNumbers(IList<Pair> pairs)
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
            UpdateResultsScores(tournament);
            //IList<Pair> pairsList = CalculatePairsOrder(tournament);
            //if (!withPairRepeats)
            //    CorrectRepeats(pairsList);
            IEnumerable<Result> newResults = CreateNewSitting(tournament);
            return newResults;
        }

        private static IEnumerable<Result> CreateNewSitting(Tournament tournament)
        {
            throw new NotImplementedException();
        }

        public static void UpdateResultsScores(Tournament tournament)
        {
            //var allBoardsNumbers = tournament.Boards.Select(b => b.BoardNumber);
            var resultsGroupedByBoards = tournament.Results.GroupBy(r => r.Board.BoardNumber, r=>r, 
                (key,g) => new { BoardId = key, Results = g.ToList()}).ToList();
            foreach (var resultsGroupedByBoard in resultsGroupedByBoards)
            {
                UpdateBoardResults(resultsGroupedByBoard.Results,tournament.TournamentScoringType);
            }
        }

        public static void UpdateBoardResults(List<Result> resultsGroupedByBoard,TournamentScoringType tournamentScoringType)
        {
            foreach (var result in resultsGroupedByBoard)
            {
                result.ResultNsPoints = CalculateResultPoints(result, DealerIsVulnerable(result));
            }
            if(tournamentScoringType == TournamentScoringType.Matchpoints)
                CalculateScoresAsMatchpoints(resultsGroupedByBoard);
            else
                throw new NotSupportedException("By now only Cavendish tournament type is supported");
        }

        private static void CalculateScoresAsMatchpoints(List<Result> resultsGroupedByBoard)
        {
            var orderedResults = resultsGroupedByBoard.OrderByDescending(r => r.ResultNsPoints).ToList();
            var total = (resultsGroupedByBoard.Count - 1);
            for (int i = 0; i < orderedResults.Count; i++)
            {
                orderedResults[i].ScoreEw = i*2;
                orderedResults[i].ScoreNs = (total - i) * 2;
            }
        }

        public static int CalculateResultPoints(Result result, bool dealerIsVulnerable)
        {
            int score = 0;
            int doubled = result.ContractDoubled == ContractDoubled.NotDoubled ? 1 : (result.ContractDoubled == ContractDoubled.Doubled ? 2 : 4);
            if(result.ContractLevel==0)
                return 0;
            if (result.NumberOfTricks >= result.ContractLevel + 6)
            {
                int pointsPerTrick = GetPointsPerTrick(result.ContractColor);
                score = pointsPerTrick*(result.NumberOfTricks - 6)*doubled;
                if (result.ContractColor == ContractColors.NoTrump)
                    score += 10;
                if (score >= 100)
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
                if (result.ContractDoubled == ContractDoubled.Doubled)
                    score += 50;
                else if (result.ContractDoubled == ContractDoubled.Redoubled)
                    score += 100;
            }
            else
            {
                int numberOfUndertricks = result.NumberOfTricks - result.ContractLevel + 6;
                if (dealerIsVulnerable)
                {
                    if (doubled == 1)
                        score -= (numberOfUndertricks*100);
                    else
                        score -= (numberOfUndertricks * 150 * doubled ) + 50 * doubled;
                }
                else
                {
                    if (doubled == 1)
                        score -= (numberOfUndertricks * 50);
                    else
                        score -= (numberOfUndertricks * 100 * doubled) + 50 * doubled;
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
    }
}
