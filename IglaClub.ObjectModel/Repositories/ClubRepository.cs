using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
    }
}