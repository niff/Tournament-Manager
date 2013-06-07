using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IglaClub.Web.Models
{
    public class IglaClubDbContext : DbContext, IIglaClubDbContext
    {
            public IglaClubDbContext()
                : base("name=IglaClubConnection")
            {
                System.Data.Entity.Database.SetInitializer(
                    new IglaClubDbContextInitializer());
                //System.Data.Entity.Database.SetInitializer(
                //    new System.Data.Entity.MigrateDatabaseToLatestVersion<IglaClubWeb.Models.IglaClubDbContext,>());
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Club> Clubs { get; set; }
            public DbSet<Tournament> Tournaments { get; set; }
            public DbSet<Pair> Pairs { get; set; }
            public DbSet<BoardDefinition> BoardDefinitions { get; set; }
            public DbSet<BoardInstance> BoardInstances { get; set; }
            public DbSet<Card> Cards { get; set; }
            public DbSet<Result> Results { get; set; }



            public class IglaClubDbContextInitializer : DropCreateDatabaseIfModelChanges<IglaClubDbContext>
            {
                protected override void Seed(IglaClubDbContext context)
                {
                    var club = new Club {Name = "Bracka", Description = "Róg brackiej i reformackiej"};
                    var userA = new User {Login = "a", Name = "a"};
                    var userB = new User {Login = "b", Name = "b"};
                    context.Clubs.Add(club);
                    context.Users.Add(userA);
                    context.Users.Add(userB);
                    context.Users.Add(new User { Login = "c", Name = "c" });
                    context.Users.Add(new User { Login = "d", Name = "d" });
                    context.Users.Add(new User { Login = "e", });
                    var tournament = new Tournament()
                        {
                            Name = "Pierwszy turniej",
                            BoardsInRound = 3,
                            Club = club,
                            Description = "Tournament description",
                            Pairs = new List<Pair>(),
                            Boards = new List<BoardInstance>(),
                            Results = new List<Result>(),
                            CreationDate = DateTime.Now
                        };
                    var pair = new Pair() {Player1 = userA, Player2 = userB, Tournament = tournament};
                    tournament.Pairs.Add(pair);
                    context.Tournaments.Add(tournament);
                    context.SaveChanges();
                }
            }

        public new int SaveChanges()
        {
           return base.SaveChanges();
        }
        
    }
}