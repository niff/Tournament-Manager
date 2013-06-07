using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IIglaClubDbContext dbContext) : base(dbContext)
        {
        }

        public List<User> GetAvailableUsersForTournament(long tournamentId)
        {
            IList<Pair> subscribedPairs = db.Tournaments.Find(tournamentId).Pairs;
            var availableUsers = 
                db.Users.Where(u =>
                !subscribedPairs.Any(
                    p => (p.Player1 != null && p.Player1.Id != u.Id) || (p.Player2 != null && p.Player2.Id != u.Id)));
            return availableUsers.ToList();
        }
    }
}
