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
            return db.Tournaments.Find(id).Pairs.ToList();
        }
    }
}
