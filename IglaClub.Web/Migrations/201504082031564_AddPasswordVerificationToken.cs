namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordVerificationToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordVerificationToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordVerificationToken");
        }
    }
}
