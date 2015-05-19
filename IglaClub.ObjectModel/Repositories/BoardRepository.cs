using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class PairRepository : BaseRepository
    {
        public PairRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public List<Pair> GetPairsByTournament(long id)
        {
            return db.Tournaments.Find(id).Pairs.ToList();
        }

        public void AddPairToTournament(Pair pair, int tournamentId)
        {
            var tournament = db.Tournaments.FirstOrDefault(t => t.Id == tournamentId);
            if (tournament != null) tournament.Pairs.Add(pair);
            db.SaveChanges();
        }
    }
}
