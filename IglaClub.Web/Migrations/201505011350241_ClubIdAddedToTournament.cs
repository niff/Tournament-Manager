namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClubIdAddedToTournament : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tournaments", "Club_Id", "dbo.Clubs");
            DropIndex("dbo.Tournaments", new[] { "Club_Id" });
            RenameColumn(table: "dbo.Tournaments", name: "Club_Id", newName: "ClubId");
            AlterColumn("dbo.Tournaments", "ClubId", c => c.Long(nullable: true));
            CreateIndex("dbo.Tournaments", "ClubId");
            AddForeignKey("dbo.Tournaments", "ClubId", "dbo.Clubs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "ClubId", "dbo.Clubs");
            DropIndex("dbo.Tournaments", new[] { "ClubId" });
            AlterColumn("dbo.Tournaments", "ClubId", c => c.Long());
            RenameColumn(table: "dbo.Tournaments", name: "ClubId", newName: "Club_Id");
            CreateIndex("dbo.Tournaments", "Club_Id");
            AddForeignKey("dbo.Tournaments", "Club_Id", "dbo.Clubs", "Id");
        }
    }
}
