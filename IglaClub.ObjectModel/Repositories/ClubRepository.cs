﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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
            
            base.InsertOrUpdate(clubUser);
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

        public IList<ClubUser> GetClubUsers(int clubId)
        {
            return db.ClubUsers.Include(cu=>cu.User).Where(cu => cu.ClubId == clubId).ToList();
        }

        public void Subscribe(long clubId, long userId)
        {

            var clubUser = new ClubUser
                {
                    Id = 0,
                    ClubId = clubId,
                    UserId = userId,
                    IsAdministrator = false,
                    MemberSince = DateTime.Now,
                    MembershipStatus = MembershipStatus.Unknown
                };


            base.InsertOrUpdate(clubUser);
            db.SaveChanges();
        }

        public void Unsubscribe(long clubId, long userId)
        {
            var item = db.ClubUsers.FirstOrDefault(c => c.ClubId == clubId && c.UserId == userId);
            this.Delete(item);
            db.SaveChanges();
        }

        public IList<Club> GetClubsByAdmin(long userId)
        {
            return db.Clubs.Where(c => c.ClubUsers.Any(cu => cu.IsAdministrator && cu.UserId == userId)).ToList();
        }
    }
}