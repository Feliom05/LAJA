namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityNavigationDeadline : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "DeadLine", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "DeadLine", c => c.DateTime(nullable: false));
        }
    }
}
