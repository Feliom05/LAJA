namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingDocumentModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FeedBack", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "FeedBack");
        }
    }
}
