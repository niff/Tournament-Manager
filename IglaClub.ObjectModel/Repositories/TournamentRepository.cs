using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    
    
    public class TournamentRepository : BaseRepository
    {
        public TournamentRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public Tournament GetTournament(int id)
        {
            return db.Tournaments
                            .Include(t => t.Results)
                            .Include(t => t.Results.Select(r => r.NS))
                            .Include(t => t.Results.Select(r => r.EW))
                            .Include(t => t.Boards)
                            .Include(t => t.Pairs).FirstOrDefault(t => t.Id == id);
        }

    }
}
