namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectedEndDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Modules", "DateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Modules", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Modules", "EndDate");
        }
    }
}
