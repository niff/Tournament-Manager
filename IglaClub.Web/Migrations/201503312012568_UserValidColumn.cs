namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserValidColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Valid", c => c.Boolean(nullable: false,defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Valid");
        }
    }
}
