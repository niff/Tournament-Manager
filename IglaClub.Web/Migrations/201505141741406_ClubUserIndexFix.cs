using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Model;

namespace IglaClub.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ClubUserIndexFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ClubUsers","Id",ColumnAction);
        }

        private ColumnModel ColumnAction(ColumnBuilder columnBuilder)
        {
            ColumnModel idColumn = columnBuilder.Long();
            idColumn.IsIdentity = true;
            idColumn.IsNullable = false;
            return idColumn;
        }

        public override void Down()
        {
        }
    }
}
