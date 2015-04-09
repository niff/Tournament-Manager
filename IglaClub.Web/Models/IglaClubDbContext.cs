using System.Data.Entity;
using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Entities;
using IglaClub.Web.Migrations;

namespace IglaClub.Web.Models
{
    public class IglaClubDbContext : DbContext, IIglaClubDbContext
    {
        public IglaClubDbContext()
            : base("name=IglaClubConnection")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<IglaClubDbContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<IglaClubDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<BoardDefinition> BoardDefinitions { get; set; }
        public DbSet<BoardInstance> BoardInstances { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ClubUser> ClubUsers { get; set; }
        public DbSet<HelpEntry> HelpEntries { get; set; }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

    }
}