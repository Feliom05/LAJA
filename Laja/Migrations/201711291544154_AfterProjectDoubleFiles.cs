namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AfterProjectDoubleFiles : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Course_Id", newName: "CourseId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Course_Id", newName: "IX_CourseId");
            AddColumn("dbo.Courses", "DocumentType_Id", c => c.Int());
            CreateIndex("dbo.Courses", "DocumentType_Id");
            AddForeignKey("dbo.Courses", "DocumentType_Id", "dbo.DocTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "DocumentType_Id", "dbo.DocTypes");
            DropIndex("dbo.Courses", new[] { "DocumentType_Id" });
            DropColumn("dbo.Courses", "DocumentType_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_CourseId", newName: "IX_Course_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "CourseId", newName: "Course_Id");
        }
    }
}
