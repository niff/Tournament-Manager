namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnToClubsUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClubUsers", "MemberSince", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClubUsers", "MemberSince");
        }
    }
}
