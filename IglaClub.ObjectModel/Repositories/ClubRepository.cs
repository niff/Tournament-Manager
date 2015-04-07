using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class ClubRepository : BaseRepository
    {
        public ClubRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public void Insert(Club club, User owner)
        {
            base.InsertOrUpdate(club);

            var clubUser = new ClubUser()
            {
                Club = club,
                User = owner,
                IsAdministrator = true
            };
            base.InsertOrUpdate(clubUser);
            
            SaveChanges();
        }

        public void Update(Club club)
        {
            base.InsertOrUpdate(club);
            SaveChanges();
        }
    }
}