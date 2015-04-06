using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class ResultRepository : BaseRepository
    {
        public ResultRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public List<Result> GetResultsByTournamentAndUser(long tournamentId, long userId)
        {
            return GetResultsByTournamentAndUserQuery(tournamentId, userId)
                                          .Include(r => r.NS)
                                          .Include(r => r.EW)
                                          .Include(r => r.Board)
                                          .Include(r => r.Board.BoardDefinition)
                                          .ToList();
        }

        public List<Result> GetResultsByTournamentAndRoundAndUser(long tournamentId, int roundNumber, long userId)
        {
            return GetResultsByTournamentAndUserQuery(tournamentId,userId).Where(r=>r.RoundNumber==roundNumber)
                                          .Include(r => r.NS)
                                          .Include(r => r.EW)
                                          .Include(r => r.Board)
                                          .Include(r => r.Board.BoardDefinition)
                                          .ToList();
        }
        private IQueryable<Result> GetResultsByTournamentAndUserQuery(long tournamentId, long userId)
        {
            return db.Results.Where(r => r.TournamentId == tournamentId &&
                                         (((r.EW.Player1 != null && r.EW.Player1.Id == userId)
                                           || (r.EW.Player2 != null && r.EW.Player2.Id == userId)
                                           || (r.NS.Player1 != null && r.NS.Player1.Id == userId)
                                           || (r.NS.Player2 != null && r.NS.Player2.Id == userId))));
        }
        public List<Result> GetResultsFromCurrentRound(long tournamentId)
        {
            var tournament = db.Tournaments.Find(tournamentId);
            return tournament.Results.Where(r => r.RoundNumber == tournament.CurrentRound).ToList();
        }

        public List<Result> GetResults(long tournamentId)
        {
            return db.Results.Where(r => r.Tournament.Id == tournamentId).ToList();
        }
        public Dictionary<long, int> GetDictionaryPairNumberMaxPoints(long tournamentId)
        {
            var res = new Dictionary<long, int>();
            var finishedResults = GetResults(tournamentId).Where(r=>r.IsFinished).ToList();
            foreach (var pair in db.Pairs.Where(p => p.Tournament.Id == tournamentId).ToList())
            {
                long pairId = pair.Id;
                int pairNumber = pair.PairNumber;
                List<int> boardsPlayedByPair =
                    finishedResults.Where(r => (r.NS.Id == pairId || r.EW.Id == pairId)).Select(r => r.Board.BoardNumber).ToList();
                var numberOfResults =
                    finishedResults.Count(r => boardsPlayedByPair.Contains(r.Board.BoardNumber));
                var maxPoints = (numberOfResults - boardsPlayedByPair.Count) * 2;
                res.Add(pairNumber, maxPoints);
            }
            return res;
        }

        


    }
}
