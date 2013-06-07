using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IglaClub.ObjectModel.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IIglaClubDbContext dbContext) : base(dbContext)
        {
        }

        public List<Entities.User> GetAvailableUsersForTournament()
        {
            throw new NotImplementedException();
        }
    }
}
