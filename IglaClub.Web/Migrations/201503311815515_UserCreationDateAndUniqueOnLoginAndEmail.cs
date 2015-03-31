namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCreationDateAndUniqueOnLoginAndEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "LastLoginTs", c => c.DateTime());
            CreateIndex("dbo.Users", "Login", unique: true);
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Login" });
            DropColumn("dbo.Users", "LastLoginTs");
            DropColumn("dbo.Users", "CreationDate");
        }
    }
}
