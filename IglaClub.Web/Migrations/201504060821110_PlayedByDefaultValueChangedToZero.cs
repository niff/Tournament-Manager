namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PlayedByDefaultValueChangedToZero : DbMigration
    {
        public override void Up()
        {
            Sql("update Results set PlayedBy = 0 where PlayedBy = 1");
            Sql("update Results set ContractDoubled = 0 where ContractDoubled = 1");
        }
        
        public override void Down()
        {
            Sql("update Results set ContractDoubled = 1 where ContractDoubled = 0");
            Sql("update Results set PlayedBy = 1 where PlayedBy = 0");
        }
    }
}
