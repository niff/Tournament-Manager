namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClubUseridFKColumns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClubUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ClubUsers", "Club_Id", "dbo.Clubs");
            DropIndex("dbo.ClubUsers", new[] { "Club_Id" });
            DropIndex("dbo.ClubUsers", new[] { "User_Id" });
            RenameColumn(table: "dbo.ClubUsers", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.ClubUsers", name: "Club_Id", newName: "ClubId");
            AlterColumn("dbo.ClubUsers", "ClubId", c => c.Long(nullable: false));
            AlterColumn("dbo.ClubUsers", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.ClubUsers", "UserId");
            CreateIndex("dbo.ClubUsers", "ClubId");
            AddForeignKey("dbo.ClubUsers", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClubUsers", "ClubId", "dbo.Clubs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClubUsers", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.ClubUsers", "UserId", "dbo.Users");
            DropIndex("dbo.ClubUsers", new[] { "ClubId" });
            DropIndex("dbo.ClubUsers", new[] { "UserId" });
            AlterColumn("dbo.ClubUsers", "UserId", c => c.Long());
            AlterColumn("dbo.ClubUsers", "ClubId", c => c.Long());
            RenameColumn(table: "dbo.ClubUsers", name: "ClubId", newName: "Club_Id");
            RenameColumn(table: "dbo.ClubUsers", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.ClubUsers", "User_Id");
            CreateIndex("dbo.ClubUsers", "Club_Id");
            AddForeignKey("dbo.ClubUsers", "Club_Id", "dbo.Clubs", "Id");
            AddForeignKey("dbo.ClubUsers", "User_Id", "dbo.Users", "Id");
        }
    }
}
