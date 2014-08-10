namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser3 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [users] values ('jb3',NULL,NULL,'jb2',NULL)");
        }
        
        public override void Down()
        {
        }
    }
}
