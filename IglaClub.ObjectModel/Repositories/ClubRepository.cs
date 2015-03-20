using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class ClubRepository : BaseRepository
    {
        public ClubRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public void Add(Club club)
        {
            base.InsertOrUpdate(club);
            SaveChanges();
        }
    }
}