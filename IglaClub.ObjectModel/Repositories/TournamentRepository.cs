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

        public Tournament GetTournamentWithInclude(long id)
        {
            return db.Tournaments
                            .Include(t => t.Results)
                            .Include(t => t.Results.Select(r => r.NS))
                            .Include(t => t.Results.Select(r => r.EW))
                            .Include(t => t.Boards)
                            .Include(t => t.Pairs)
                            .FirstOrDefault(t => t.Id == id);
        }
        public Tournament GetTournament(long id)
        {
            return db.Tournaments
                            .FirstOrDefault(t => t.Id == id);
        }


        public IEnumerable<Tournament> GetOncoming()
        {
            return db.Tournaments.Where(t => t.TournamentStatus == Enums.TournamentStatus.Planned);
        }

        public IEnumerable<Tournament> GetPast()
        {
            return db.Tournaments.Where(t => t.TournamentStatus == Enums.TournamentStatus.Finished);
        }

        public IEnumerable<Tournament> GetOngoing()
        {
            return db.Tournaments.Where(t => t.TournamentStatus == Enums.TournamentStatus.Started);
        }

        public bool UserIsTournamentOwner(string userName, long tournamentId)
        {
            var tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null || tournament.Owner == null || string.Compare(tournament.Owner.Login, userName, true) != 0)
                return false;
            return true;

            
            
        }
    }
}
