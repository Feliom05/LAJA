namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingDocumentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DeadLineFixed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "IsShared", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "Path", c => c.String());
            DropColumn("dbo.Documents", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Name", c => c.String());
            DropColumn("dbo.Documents", "Path");
            DropColumn("dbo.Documents", "IsShared");
            DropColumn("dbo.Documents", "DeadLineFixed");
        }
    }
}
