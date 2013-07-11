using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class ResultRepository :BaseRepository
    {
        public ResultRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public List<Result> GetResults(long tournamentId, int roundNumber, int tableNumber)
        {
            throw new NotImplementedException();
        }

        public List<Result> GetResults(long tournamentId)
        {
            return db.Results.Where(r => r.Tournament.Id == tournamentId).ToList();
        }
        public Dictionary<long, int> GetDictionaryPairNumberMaxPoints(int tournamentId)
        {
            var  res = new Dictionary<long, int>();
            var results = GetResults(tournamentId);
            foreach (var pair in db.Pairs.Where(p=>p.Tournament.Id == tournamentId).ToList())
            {
                long pairId = pair.Id;
                int pairNumber = pair.PairNumber;
                List<int> boardsPlayedByPair =
                    results.Where(r => (r.ResultNsPoints != null && (r.NS.Id == pairId || r.EW.Id == pairId))).Select(r=>r.Board.BoardNumber).ToList();
                var numberOfResults =
                    results.Count(r => (r.ResultNsPoints != null) && boardsPlayedByPair.Contains(r.Board.BoardNumber));
                var maxPoints = (numberOfResults - boardsPlayedByPair.Count)*2;
                res.Add(pairNumber,maxPoints);
            }
            return res;
        }

        
    }
}
