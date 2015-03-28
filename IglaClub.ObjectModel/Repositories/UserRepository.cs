using System.Collections.Generic;
using System.Linq;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IIglaClubDbContext dbContext) : base(dbContext)
        {
        }

        public long Add(string login, string email)
        {
            var user = new User
                {
                    Login = login,
                    Nickname = login,
                    Name = login,
                    Email = email
                };
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public bool EmailExistInDataBase(string email)
        {
            return db.Users.Any(u => u.Email == email);
        }
        public bool UserExistInDataBase(string userName)
        {
            return db.Users.Any(u => u.Name == userName);
        }

        public List<User> GetAvailableUsersForTournament(long tournamentId)
        {
            //todo b: filter by club users
            List<long> subscribedPairUsersIds = ( from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                    where pairs.Player1 != null
                                                    select pairs.Player1.Id)
                                                   .Union(from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                                                  where pairs.Player2 != null
                                                                                  select pairs.Player2.Id).ToList();
            var availableUsers = db.Users.Where(u => !subscribedPairUsersIds.Contains(u.Id)).OrderByDescending(p=>p.Nickname).ToList();
            return availableUsers;
        }

        public List<User> GetUsersByPhraseAndTournament(long tournamentId, string phrase)
        {
            //todo: filter by club users
            phrase = phrase.ToLowerInvariant();
            List<long> subscribedPairUsersIds = ( from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                    where pairs.Player1 != null
                                                    select pairs.Player1.Id)
                                                   .Union(from pairs in db.Tournaments.Find(tournamentId).Pairs
                                                                                  where pairs.Player2 != null
                                                                                  select pairs.Player2.Id).ToList();
            List<User> matchedUsers =
                db.Users
                .Where(u => !subscribedPairUsersIds.Contains(u.Id))
                .Where(u => u.Name.ToLower().Contains(phrase) || u.Lastname.ToLower().Contains(phrase)
                    || u.Login.ToLower().Contains(phrase))
                .ToList();
            return matchedUsers;
        }

        public User GetUserByLogin(string userLogin)
        {
            return db.Users.FirstOrDefault(u => System.String.Compare(u.Login, userLogin, System.StringComparison.OrdinalIgnoreCase) == 0);
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => System.String.Compare(u.Email, email, System.StringComparison.OrdinalIgnoreCase) == 0);
        }

        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public void InsertOrUpdate(User user)
        {
            base.InsertOrUpdate(user);
            db.SaveChanges();
        }
    }
}
