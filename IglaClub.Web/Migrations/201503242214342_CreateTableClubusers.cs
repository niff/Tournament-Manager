namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableClubusers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClubUsers", "IsAdministrator", c => c.Boolean()); 
        }
        
        public override void Down()
        {
            
        }
    }
}
