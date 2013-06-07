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

        public List<Pair> GetPairsByTournament(int id)
        {
            var tournament = db.Tournaments.Include(t => t.Pairs).FirstOrDefault(t => t.Id == id);
            if (tournament != null)
                return tournament.Pairs.ToList();
            return null;
        }
    }
}
