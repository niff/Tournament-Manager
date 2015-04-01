namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEmailColumnToAcceptNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("users","email", c=>c.String(nullable : true));
        }
        
        public override void Down()
        {
        }
    }
}
