using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Repositories;

namespace IglaClub.Web.Controllers
{
    public class HelpRepository : BaseRepository
    {
        public HelpRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}