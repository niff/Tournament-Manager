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
                    context.Clubs.Add(new Club { Name = "Bracka", Description = "Róg brackiej i reformackiej" });
                    context.Users.Add(new User { Login = "a" });
                    context.Users.Add(new User { Login = "b" });
                    context.Users.Add(new User { Login = "c" });
                    context.Users.Add(new User { Login = "d" });
                    context.Users.Add(new User { Login = "e" });
                    
                    
                    context.SaveChanges();
                }
            }
        
    }
}