namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePasswordVerificationToken : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "PasswordVerificationToken");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PasswordVerificationToken", c => c.String());
        }
    }
}
