namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            AddColumn("dbo.AspNetUsers", "Course_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Course_Id");
            AddForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Courses", new[] { "Name" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_Id" });
            DropColumn("dbo.AspNetUsers", "Course_Id");
            DropTable("dbo.Courses");
        }
    }
}
