namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [users] values ('jb1',NULL,NULL,'jb1',NULL)");
            
        }
        
        public override void Down()
        {
        }
    }
}
