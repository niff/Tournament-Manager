namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class newTournament : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [IglaClub].[dbo].[Tournaments]
           ([Name]
           ,[Description]
           ,[TournamentScoringType]
           ,[TournamentMovingType]
           ,[BoardsInRound]
           ,[TournamentStatus]
           ,[CurrentRound]
           ,[PlannedStartDate]
           ,[CreationDate]
           ,[StartDate]
           ,[FinishDate]
           ,[Club_Id]
           ,[OwnerId])
     VALUES
           ('migracyjny'
           ,'migracyjny turniej srodowy'
           ,0
           ,0
           ,3
           ,1
           ,0
           ,'2015-03-19 19:00:00.000'
           ,'2015-03-18 19:00:00.000'
           ,null
           ,null
           ,1
           ,1)");
        }
        
        public override void Down()
        {
        }
    }
}
