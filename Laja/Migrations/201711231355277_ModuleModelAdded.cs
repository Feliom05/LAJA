namespace Laja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuleModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        CourseId = c.Int(nullable: false),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => new { t.Name, t.CourseId }, unique: true, name: "IX_UniqeModelName");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            DropIndex("dbo.Modules", "IX_UniqeModelName");
            DropTable("dbo.Modules");
        }
    }
}
