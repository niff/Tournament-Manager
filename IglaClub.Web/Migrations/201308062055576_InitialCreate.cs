namespace IglaClub.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Lastname = c.String(maxLength: 100),
                        Nickname = c.String(maxLength: 100),
                        Login = c.String(maxLength: 100),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        TournamentScoringType = c.Int(nullable: false),
                        TournamentMovingType = c.Int(nullable: false),
                        BoardsInRound = c.Int(nullable: false),
                        TournamentStatus = c.Int(nullable: false),
                        CurrentRound = c.Int(nullable: false),
                        PlannedStartDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        Club_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clubs", t => t.Club_Id)
                .Index(t => t.Club_Id);
            
            CreateTable(
                "dbo.Pairs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PairNumber = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                        Tournament_Id = c.Long(),
                        Player1_Id = c.Long(),
                        Player2_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id)
                .ForeignKey("dbo.Users", t => t.Player1_Id)
                .ForeignKey("dbo.Users", t => t.Player2_Id)
                .Index(t => t.Tournament_Id)
                .Index(t => t.Player1_Id)
                .Index(t => t.Player2_Id);
            
            CreateTable(
                "dbo.BoardInstances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BoardNumber = c.Int(nullable: false),
                        Tournament_Id = c.Long(),
                        BoardDefinition_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id)
                .ForeignKey("dbo.BoardDefinitions", t => t.BoardDefinition_Id)
                .Index(t => t.Tournament_Id)
                .Index(t => t.BoardDefinition_Id);
            
            CreateTable(
                "dbo.BoardDefinitions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Dealer = c.Int(nullable: false),
                        Vulnerability = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Color = c.Int(nullable: false),
                        Value = c.String(),
                        BoardDefinition_Id = c.Long(),
                        BoardDefinition_Id1 = c.Long(),
                        BoardDefinition_Id2 = c.Long(),
                        BoardDefinition_Id3 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BoardDefinitions", t => t.BoardDefinition_Id)
                .ForeignKey("dbo.BoardDefinitions", t => t.BoardDefinition_Id1)
                .ForeignKey("dbo.BoardDefinitions", t => t.BoardDefinition_Id2)
                .ForeignKey("dbo.BoardDefinitions", t => t.BoardDefinition_Id3)
                .Index(t => t.BoardDefinition_Id)
                .Index(t => t.BoardDefinition_Id1)
                .Index(t => t.BoardDefinition_Id2)
                .Index(t => t.BoardDefinition_Id3);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContractLevel = c.Int(nullable: false),
                        ContractColor = c.Int(nullable: false),
                        ContractDoubled = c.Int(nullable: false),
                        PlayedBy = c.Int(nullable: false),
                        NumberOfTricks = c.Int(nullable: false),
                        ResultNsPoints = c.Int(),
                        Board_Id = c.Long(nullable: false),
                        Tournament_Id = c.Long(nullable: false),
                        RoundNumber = c.Int(nullable: false),
                        TableNumber = c.Int(nullable: false),
                        ScoreNs = c.Single(),
                        ScoreEw = c.Single(),
                        NS_Id = c.Long(),
                        EW_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pairs", t => t.NS_Id)
                .ForeignKey("dbo.Pairs", t => t.EW_Id)
                .ForeignKey("dbo.BoardInstances", t => t.Board_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.NS_Id)
                .Index(t => t.EW_Id)
                .Index(t => t.Board_Id)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.ClubUsers",
                c => new
                    {
                        Club_Id = c.Long(nullable: false),
                        User_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Club_Id, t.User_Id })
                .ForeignKey("dbo.Clubs", t => t.Club_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Club_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClubUsers", new[] { "User_Id" });
            DropIndex("dbo.ClubUsers", new[] { "Club_Id" });
            DropIndex("dbo.Results", new[] { "Tournament_Id" });
            DropIndex("dbo.Results", new[] { "Board_Id" });
            DropIndex("dbo.Results", new[] { "EW_Id" });
            DropIndex("dbo.Results", new[] { "NS_Id" });
            DropIndex("dbo.Cards", new[] { "BoardDefinition_Id3" });
            DropIndex("dbo.Cards", new[] { "BoardDefinition_Id2" });
            DropIndex("dbo.Cards", new[] { "BoardDefinition_Id1" });
            DropIndex("dbo.Cards", new[] { "BoardDefinition_Id" });
            DropIndex("dbo.BoardInstances", new[] { "BoardDefinition_Id" });
            DropIndex("dbo.BoardInstances", new[] { "Tournament_Id" });
            DropIndex("dbo.Pairs", new[] { "Player2_Id" });
            DropIndex("dbo.Pairs", new[] { "Player1_Id" });
            DropIndex("dbo.Pairs", new[] { "Tournament_Id" });
            DropIndex("dbo.Tournaments", new[] { "Club_Id" });
            DropForeignKey("dbo.ClubUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ClubUsers", "Club_Id", "dbo.Clubs");
            DropForeignKey("dbo.Results", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Results", "Board_Id", "dbo.BoardInstances");
            DropForeignKey("dbo.Results", "EW_Id", "dbo.Pairs");
            DropForeignKey("dbo.Results", "NS_Id", "dbo.Pairs");
            DropForeignKey("dbo.Cards", "BoardDefinition_Id3", "dbo.BoardDefinitions");
            DropForeignKey("dbo.Cards", "BoardDefinition_Id2", "dbo.BoardDefinitions");
            DropForeignKey("dbo.Cards", "BoardDefinition_Id1", "dbo.BoardDefinitions");
            DropForeignKey("dbo.Cards", "BoardDefinition_Id", "dbo.BoardDefinitions");
            DropForeignKey("dbo.BoardInstances", "BoardDefinition_Id", "dbo.BoardDefinitions");
            DropForeignKey("dbo.BoardInstances", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Pairs", "Player2_Id", "dbo.Users");
            DropForeignKey("dbo.Pairs", "Player1_Id", "dbo.Users");
            DropForeignKey("dbo.Pairs", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "Club_Id", "dbo.Clubs");
            DropTable("dbo.ClubUsers");
            DropTable("dbo.Results");
            DropTable("dbo.Cards");
            DropTable("dbo.BoardDefinitions");
            DropTable("dbo.BoardInstances");
            DropTable("dbo.Pairs");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Clubs");
            DropTable("dbo.Users");
        }
    }
}
