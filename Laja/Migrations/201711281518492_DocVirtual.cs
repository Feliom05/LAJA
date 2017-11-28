namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocVirtual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "DocumentType_Id", c => c.Int());
            CreateIndex("dbo.Courses", "DocumentType_Id");
            AddForeignKey("dbo.Courses", "DocumentType_Id", "dbo.DocTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "DocumentType_Id", "dbo.DocTypes");
            DropIndex("dbo.Courses", new[] { "DocumentType_Id" });
            DropColumn("dbo.Courses", "DocumentType_Id");
        }
    }
}
