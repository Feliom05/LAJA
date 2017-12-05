namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigrations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "Description", c => c.String());
        }
    }
}
