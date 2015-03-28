namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClubUserRenameColumn : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ClubUsers", name: "Users_Id", newName: "User_Id");
            RenameColumn(table: "dbo.ClubUsers", name: "Clubs_Id", newName: "Club_Id");
            RenameIndex(table: "dbo.ClubUsers", name: "IX_Clubs_Id", newName: "IX_Club_Id");
            RenameIndex(table: "dbo.ClubUsers", name: "IX_Users_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Club", name: "IX_Users_Id", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ClubUsers", name: "IX_User_Id", newName: "IX_Users_Id");
            RenameIndex(table: "dbo.ClubUsers", name: "IX_Club_Id", newName: "IX_Clubs_Id");
            RenameColumn(table: "dbo.ClubUsers", name: "Club_Id", newName: "Clubs_Id");
            RenameColumn(table: "dbo.ClubUsers", name: "User_Id", newName: "Users_Id");
        }
    }
}
