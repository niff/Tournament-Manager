namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIndexClubUser : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.ClubUsers", "Club_Id","dbo.Clubs", "Id",name:"IX_Club_Id");
            AddForeignKey("dbo.ClubUsers", "User_Id","dbo.Users", "Id",name:"IX_User_Id");
        }
        
        public override void Down()
        {
        }
    }
}
