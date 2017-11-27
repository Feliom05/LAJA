namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        ModuleId = c.Int(nullable: false),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                        SubmitRequired = c.Boolean(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityTypes", t => t.ActivityTypeId)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .Index(t => new { t.Name, t.ModuleId }, unique: true, name: "IX_UniqeActivityName")
                .Index(t => t.ActivityTypeId);
            
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.DocTypes", t => t.DocTypeId)
                .Index(t => t.FileName, unique: true, name: "IX_UniqeFileName")
                .Index(t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.ModuleId)
                .Index(t => t.ActivityId)
                .Index(t => t.DocTypeId);
            
            AddForeignKey("dbo.Modules", "CourseId", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.Documents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Documents", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Documents", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Activities", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropIndex("dbo.Documents", new[] { "DocTypeId" });
            DropIndex("dbo.Documents", new[] { "ActivityId" });
            DropIndex("dbo.Documents", new[] { "ModuleId" });
            DropIndex("dbo.Documents", new[] { "CourseId" });
            DropIndex("dbo.Documents", new[] { "UserId" });
            DropIndex("dbo.Documents", "IX_UniqeFileName");
            DropIndex("dbo.DocTypes", new[] { "Extension" });
            DropIndex("dbo.Activities", new[] { "ActivityTypeId" });
            DropIndex("dbo.Activities", "IX_UniqeActivityName");
            DropTable("dbo.Documents");
            DropTable("dbo.DocTypes");
            DropTable("dbo.ActivityTypes");
            DropTable("dbo.Activities");
            AddForeignKey("dbo.Modules", "CourseId", "dbo.Courses", "Id", cascadeDelete: true);
        }
    }
}
