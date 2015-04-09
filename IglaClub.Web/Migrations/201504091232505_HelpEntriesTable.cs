namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpEntriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HelpEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            Sql("insert into dbo.HelpEntries values ('During the fourth round of cavendish one pair has decided to go home','Just easily go to manage tournament -> show details -> edit and remove this pair. \n\rIf there are some boards that are generated to play by them, please go to the edit results and change played by to director score. Nextly, you should write NS and EW score from this board.');");
            Sql("insert into dbo.HelpEntries values ('I want to subscribe a player which does not have an accout in the system','No definition yet.');");
            Sql("insert into dbo.HelpEntries values ('Somebody came during first round','No definition yet.');");
        }
        
        public override void Down()
        {
            DropTable("dbo.HelpEntries");
        }
    }
}
