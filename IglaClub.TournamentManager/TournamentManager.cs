using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using IglaClub.ObjectModel.Tools;


namespace IglaClub.TournamentManager
{
    public class TournamentManager
    {
        private readonly IIglaClubDbContext db;

        public TournamentManager(IIglaClubDbContext dbContext)
        {
            this.db = dbContext;
        }

        public bool AddPair(long tournamentId, long player1Id, long player2Id)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            User player1 = db.Users.Find(player1Id);
            User player2 = db.Users.Find(player2Id);
            tournament.Pairs.Add(new Pair() { Tournament = tournament, PairNumber = 1, Player1 = player1, Player2 = player2} );
            db.SaveChanges();
            return true;
        }

        public bool StartTournament(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            List<BoardInstance> boards = GenerateBoards(tournament);
            AddBoardsToTournament(tournament, boards);

            List<Result> results = GenerateInitialSittingPosition(tournament);
            AddResultsToTournament(tournament, results);
            
            tournament.CurrentRound = 1;
            tournament.TournamentStatus = TournamentStatus.Started;
            tournament.StartDate = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        private void AddResultsToTournament(Tournament tournament, IEnumerable<Result> results)
        {
            foreach (var result in results)
            {
                tournament.Results.Add(result);
            }
        }

        private static List<Result> GenerateInitialSittingPosition(Tournament tournament)
        {
            var results = new List<Result>();
            CorrectPairNumbers(tournament.Pairs);
            if(tournament.Pairs.Count%2 == 1)
                tournament.Pairs.Add(new Pair() { Player1 = null, Player2 = null, PairNumber = tournament.Pairs.Count +1});

            for (int i = 0; i < tournament.Pairs.Count/2; i++)
            {
                for (int j = 0; j < tournament.BoardsInRound; j++)
                {
                    var result = new Result()
                        {
                            Tournament = tournament,
                            Board = tournament.Boards[j],
                            NS = tournament.Pairs[i],
                            EW = tournament.Pairs[i + tournament.Pairs.Count],
                            RoundNumber = 1
                        };
                    results.Add(result);      
                }  
            }
            return results;
        }

        private static void CorrectPairNumbers(IList<Pair> pairs)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                pairs[i].PairNumber = i+1;

            }
        }

        private static void AddBoardsToTournament(Tournament tournament, IEnumerable<BoardInstance> boards)
        {
            foreach (var boardInstance in boards)
            {
                tournament.Boards.Add(boardInstance);
            }
        }

        private List<BoardInstance> GenerateBoards(Tournament tournament)
        {
            var boardInstances = new List<BoardInstance>();
            if (tournament.TournamentType != TournamentTypes.Cavendish)
                throw new NotSupportedException("By now only Cavendish tournament type is supported");
            
            for (int i = 1; i < tournament.BoardsInRound+1; i++)
            {
                boardInstances.Add(CreateEmptyBoardInstance(tournament, i));
            }

            return boardInstances;
        }

        private BoardInstance CreateEmptyBoardInstance(Tournament tournament, int boardNumber)
        {
            var boardDefinition = new BoardDefinition()
                {
                    CreationDate = DateTime.Now,
                    Dealer = GetDealerByBoardNumber(boardNumber),
                    Vulnerability = GetVulneralbityByBoardNumber(boardNumber)
                };
            db.BoardDefinitions.Add(boardDefinition);
            db.SaveChanges();

            var boardInstance = new BoardInstance()
                {
                    BoardDefinition = boardDefinition,
                    BoardNumber = boardNumber,
                    Tournament = tournament
                };
            //db.BoardInstances.Add(boardInstance);
            //db.SaveChanges();
            return boardInstance;
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
