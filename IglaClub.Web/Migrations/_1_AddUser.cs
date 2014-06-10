using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace IglaClub.Web.Migrations
{
    public class _1_AddUser : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [users] values ('jb1',NULL,NULL,'jb1',NULL)");
        }
    }
}