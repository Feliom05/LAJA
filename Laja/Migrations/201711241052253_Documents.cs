namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Documents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        FileName = c.String(nullable: false, maxLength: 255),
                        CreationTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CourseId = c.Int(),
                        ModuleId = c.Int(),
                        ActivityId = c.Int(),
                        DocTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.FileName, unique: true, name: "IX_UniqeFileName")
                .Index(t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.ModuleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Documents", "CourseId", "dbo.Courses");
            DropIndex("dbo.Documents", new[] { "ModuleId" });
            DropIndex("dbo.Documents", new[] { "CourseId" });
            DropIndex("dbo.Documents", new[] { "UserId" });
            DropIndex("dbo.Documents", "IX_UniqeFileName");
            DropTable("dbo.Documents");
        }
    }
}
