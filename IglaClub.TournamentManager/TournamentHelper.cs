using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;

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

    }
}
