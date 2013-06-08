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
            //todo: filter by club users
            List<long> subscribedPairUsersIds = ( from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                    where pairs.Player1 != null
                                                    select pairs.Player1.Id)
                                                   .Union(from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                                                  where pairs.Player2 != null
                                                                                  select pairs.Player2.Id).ToList();
            var availableUsers = db.Users.Where(u => !subscribedPairUsersIds.Contains(u.Id)).ToList();
            return availableUsers;
        }

        public List<User> GetUsersByStringAndTournament(long tournamentId, string userPartName)
        {
            //todo: filter by club users
            List<long> subscribedPairUsersIds = ( from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                    where pairs.Player1 != null
                                                    select pairs.Player1.Id)
                                                   .Union(from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                                                  where pairs.Player2 != null
                                                                                  select pairs.Player2.Id).ToList();
            List<User> matchedUsers =
                db.Users
                .Where(u => !subscribedPairUsersIds.Contains(u.Id))
                .Where(u =>u.Name.Contains(userPartName) || u.Lastname.Contains(userPartName) || u.Login.Contains(userPartName))
                .ToList();
            return matchedUsers;
        }
    }
}
