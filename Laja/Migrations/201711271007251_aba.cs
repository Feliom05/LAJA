namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aba : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes");
            AlterColumn("dbo.Activities", "DeadLine", c => c.DateTime());
            CreateIndex("dbo.Documents", "ActivityId");
            AddForeignKey("dbo.Documents", "ActivityId", "dbo.Activities", "Id");
            AddForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.Documents", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Documents", new[] { "ActivityId" });
            AlterColumn("dbo.Activities", "DeadLine", c => c.DateTime(nullable: false));
            AddForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes", "Id", cascadeDelete: true);
        }
    }
}
