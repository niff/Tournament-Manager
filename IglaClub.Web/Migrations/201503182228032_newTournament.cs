namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class newTournament : DbMigration
    {
        public override void Up()
        {
            //insert into Tournaments values ('Tournament in Zurich', 'Tournament in Zurich from Swiss Series of Bridge',0,0,3,1,0,'2015-04-03 18:00:00.000', '2015-04-01 19:10:00.000', null,null,1,null,'(47.3686498, 8.539182500000038)','Zurych, Szwajcaria')

//            Sql(@"INSERT INTO [IglaClub].[dbo].[Tournaments]
//           ([Name]
//           ,[Description]
//           ,[TournamentScoringType]
//           ,[TournamentMovingType]
//           ,[BoardsInRound]
//           ,[TournamentStatus]
//           ,[CurrentRound]
//           ,[PlannedStartDate]
//           ,[CreationDate]
//           ,[StartDate]
//           ,[FinishDate]
//           ,[Club_Id]
//           ,[OwnerId])
//     VALUES
//           ('migracyjny'
//           ,'migracyjny turniej srodowy'
//           ,0
//           ,0
//           ,3
//           ,1
//           ,0
//           ,'2015-03-19 19:00:00.000'
//           ,'2015-03-18 19:00:00.000'
//           ,null
//           ,null
//           ,1
//           ,1)");
//            Sql(@"USE [IglaClub]
//GO
//SET IDENTITY_INSERT [dbo].[Users] ON 

//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (1, N'Bartlomiej', N'Igla', NULL, N'bartlomiej@igla.pl', N'bartlomiej@igla.pl', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (2, NULL, NULL, NULL, N'Particio Zimmerman', N'Particio Zimmerman', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (3, NULL, NULL, NULL, N'Dawid Slomczynski', N'Dawid Slomczynski', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (4, N'Kamil', N'Dickens', NULL, N'kamil', N'kamil', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (6, NULL, NULL, NULL, N'George Second', N'George Second', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (7, NULL, NULL, NULL, N'Katrina second', N'Katrina second', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (8, N'Justyna', N'Bogucka', NULL, N'Justyna Bogucka', N'Justyna Bogucka', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (9, NULL, NULL, NULL, N'David Beckham', N'David Beckham', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (10, NULL, NULL, NULL, N'Ryan Giggs', N'Ryan Giggs', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (11, NULL, NULL, NULL, N'Darren Flecher', N'Darren Flecher', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (12, N'Ju', N'Bogucka', NULL, N'ju.bogucka@gmail.com', N'ju.bogucka@gmail.com', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (13, NULL, NULL, NULL, N'jstocker@letec-it.ch', N'jstocker@letec-it.ch', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (14, NULL, NULL, NULL, N'karolina', N'karolina', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//INSERT [dbo].[Users] ([Id], [Name], [Lastname], [Nickname], [Login], [Email], [CreationDate], [LastLoginTs]) VALUES (15, NULL, NULL, NULL, N'bartek', N'bartek', CAST(N'1900-01-01 00:00:00.000' AS DateTime), NULL)
//GO
//SET IDENTITY_INSERT [dbo].[Users] OFF
//GO
//");

//            Sql(@"  delete from Results where NS_Id>13 or EW_Id>13
//  delete from pairs where Player1_Id >13 or Player2_Id>13
//  delete from users where id>13");
        }
        
        public override void Down()
        {
        }
    }
}
