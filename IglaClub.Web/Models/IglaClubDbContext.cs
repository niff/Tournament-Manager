using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using IglaClub.ObjectModel.Enums;

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
                    var userA = new User {Login = "antoni", Name = "antek"};
                    var userB = new User {Login = "bartek", Name = "bartłomiej"};
                    var userC = new User { Login = "cieszka", Name = "ciesława" };
                    var userD = new User { Login = "darek", Name = "dariusz" };
                    context.Clubs.Add(club);
                    context.Users.Add(userA);
                    context.Users.Add(userB);
                    context.Users.Add(userC);
                    context.Users.Add(userD);
                    context.Users.Add(new User { Login = "ela", Name = "elżbieta" });
                    context.Users.Add(new User { Login = "elzbieeta", Name = "elzbieeta" });
                    context.Users.Add(new User { Login = "igiel", Name = "Bartek Igla" });
                    context.Users.Add(new User { Login = "justka", Name = "Justyna Bogucka" });
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
                    var pair1 = new Pair() {Player1 = userA, Player2 = userB, Tournament = tournament, PairNumber = 1};
                    var pair2 = new Pair() { Player1 = userC, Player2 = userD, Tournament = tournament, PairNumber = 2};
                    tournament.Pairs.Add(pair1);
                    tournament.Pairs.Add(pair2);
                    context.Tournaments.Add(tournament);

                    var t = new Tournament()
                    {
                        Name = "Czerwcowy",
                        BoardsInRound = 10,
                        Club = club,
                        Description = "Tournament rocks",
                        Pairs = new List<Pair>(),
                        Boards = new List<BoardInstance>(),
                        Results = new List<Result>(),
                        CreationDate = DateTime.Now,
                        TournamentStatus = TournamentStatus.Planned

                    };
                    context.Tournaments.Add(t);
                    context.SaveChanges();
                }
            }

        public new int SaveChanges()
        {
           return base.SaveChanges();
        }
        
    }
}