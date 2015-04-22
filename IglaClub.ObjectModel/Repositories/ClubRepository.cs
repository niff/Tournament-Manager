using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;

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
            var user = db.Users.Find(owner.Id);
            
            //club.ClubUsers = new List<ClubUser>();

            base.InsertOrUpdate(club);
            SaveChanges();

            var find = db.Clubs.Find(club.Id);
            var clubUser = new ClubUser()
            {
                ClubId = find.Id,
                UserId = user.Id,
                IsAdministrator = true,
                MemberSince = DateTime.UtcNow
            };
            //db.ClubUsers.Add(clubUser);
            //db.Entry(clubUser).State = EntityState.Added;
            //db.Refresh(RefreshMode.ClientWins, clubUser);
            base.InsertOrUpdate(clubUser);
            //club.ClubUsers.Add(clubUser);
            db.ClubUsers.Add(clubUser);
            SaveChanges();
        }

        public void Update(Club club)
        {
            base.InsertOrUpdate(club);
            SaveChanges();
        }

        public IEnumerable<User> GetClubMembers(int clubId)
        {
            List<long> clubUsers = db.ClubUsers.Where(cu => cu.ClubId == clubId).Select(cu => cu.UserId).ToList();
            return db.Users.Where(u => clubUsers.Contains(u.Id));
        }

        public void Subscribe(long clubId, long userId)
        {
            this.InsertOrUpdate(new ClubUser
                {
                    ClubId = clubId,
                    UserId = userId,
                    IsAdministrator = false,
                    MemberSince = DateTime.Now,
                    MembershipStatus = MembershipStatus.Unknown
                });

            db.SaveChanges();
        }

        public void Unsubscribe(long id, long userId)
        {
            var item = db.ClubUsers.FirstOrDefault(c => c.ClubId == id && c.UserId == userId);
            this.Delete(item);
            db.SaveChanges();
        }
    }
}