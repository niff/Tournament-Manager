namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ClubUseridColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("ClubUsers","Id",c=>c.Long());
        }
        
        public override void Down()
        {
        }
    }
}
