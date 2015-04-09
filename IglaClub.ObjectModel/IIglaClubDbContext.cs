using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel
{
    public interface IIglaClubDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Club> Clubs { get; set; }
        DbSet<Tournament> Tournaments { get; set; }
        DbSet<Pair> Pairs { get; set; }
        DbSet<BoardDefinition> BoardDefinitions { get; set; }
        DbSet<BoardInstance> BoardInstances { get; set; }
        DbSet<Card> Cards { get; set; }
        DbSet<Result> Results { get; set; }
        DbSet<ClubUser> ClubUsers { get; set; }
        DbSet<HelpEntry> HelpEntries { get; set; }
        int SaveChanges();

        DbEntityEntry Entry(object entity);
    }
}
