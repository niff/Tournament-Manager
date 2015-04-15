namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ClubUseridColumn1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClubUsers", "MembershipStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Clubs", "CreationDate", c => c.DateTime(nullable: false, storeType:"DateTime2"));
            Sql("ALTER TABLE dbo.ClubUsers DROP CONSTRAINT DF__ClubUsers__Membe__656C112C");
            AlterColumn("dbo.ClubUsers","MemberSince",c=>c.DateTime(storeType:"DateTime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clubs", "CreationDate");
            DropColumn("dbo.ClubUsers", "MembershipStatus");
        }
    }
}
