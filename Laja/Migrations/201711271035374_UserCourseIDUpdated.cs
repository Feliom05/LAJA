namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCourseIDUpdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            RenameColumn(table: "dbo.AspNetUsers", name: "Course_Id", newName: "CourseId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Course_Id", newName: "IX_CourseId");
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
            
            CreateIndex("dbo.Documents", "ActivityId");
            CreateIndex("dbo.Documents", "DocTypeId");
            AddForeignKey("dbo.Documents", "ActivityId", "dbo.Activities", "Id");
            AddForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes", "Id");
            AddForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.Documents", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Documents", new[] { "DocTypeId" });
            DropIndex("dbo.Documents", new[] { "ActivityId" });
            DropIndex("dbo.DocTypes", new[] { "Extension" });
            DropTable("dbo.DocTypes");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_CourseId", newName: "IX_Course_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "CourseId", newName: "Course_Id");
            AddForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes", "Id", cascadeDelete: true);
        }
    }
}
