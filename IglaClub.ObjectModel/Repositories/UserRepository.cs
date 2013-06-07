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
            List<long> subscribedPairsIds = ( from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                    where pairs.Player1 != null
                                                    select pairs.Player1.Id)
                                                   .Union(from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                                                  where pairs.Player2 != null
                                                                                  select pairs.Player2.Id).ToList();
            var availableUsers = db.Users.Where(u => !subscribedPairsIds.Contains(u.Id)).ToList();
            return availableUsers;
        }
    }
}
