namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser2 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [users] values ('jb2',NULL,NULL,'jb2',NULL)");
        }
        
        public override void Down()
        {
        }
    }
}
