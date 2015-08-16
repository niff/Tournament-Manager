using System.Linq;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class BoardRepository : BaseRepository
    {
        public BoardRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public BoardInstance GetBoardByTournamentAndBoardNumber(long tournamentId, long boardNumber)
        {
            return db.Tournaments.Find(tournamentId).Boards.FirstOrDefault(b=>b.BoardNumber == boardNumber);
        }

    }
}
