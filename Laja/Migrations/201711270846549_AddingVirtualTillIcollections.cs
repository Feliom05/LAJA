namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingVirtualTillIcollections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Extension = c.String(nullable: false, maxLength: 10),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Extension, unique: true);
            
            CreateIndex("dbo.Documents", "DocTypeId");
            AddForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes");
            DropIndex("dbo.Documents", new[] { "DocTypeId" });
            DropIndex("dbo.DocTypes", new[] { "Extension" });
            DropTable("dbo.DocTypes");
        }
    }
}
