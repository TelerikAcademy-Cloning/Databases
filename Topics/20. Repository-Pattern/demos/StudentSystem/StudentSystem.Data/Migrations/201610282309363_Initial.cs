namespace StudentSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseIntance",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CourseName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Homework",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileUrl = c.String(),
                        TimeSent = c.DateTime(nullable: false),
                        StudentIdentification = c.Int(nullable: false),
                        CourseId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseIntance", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentIdentification, cascadeDelete: true)
                .Index(t => t.StudentIdentification)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Course_Id = c.Guid(),
                        Test_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseIntance", t => t.Course_Id)
                .ForeignKey("dbo.Tests", t => t.Test_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Test_Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseIntance", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Test_Id", "dbo.Tests");
            DropForeignKey("dbo.Tests", "CourseId", "dbo.CourseIntance");
            DropForeignKey("dbo.Students", "Course_Id", "dbo.CourseIntance");
            DropForeignKey("dbo.Homework", "StudentIdentification", "dbo.Students");
            DropForeignKey("dbo.Homework", "CourseId", "dbo.CourseIntance");
            DropIndex("dbo.Tests", new[] { "CourseId" });
            DropIndex("dbo.Students", new[] { "Test_Id" });
            DropIndex("dbo.Students", new[] { "Course_Id" });
            DropIndex("dbo.Homework", new[] { "CourseId" });
            DropIndex("dbo.Homework", new[] { "StudentIdentification" });
            DropTable("dbo.Tests");
            DropTable("dbo.Students");
            DropTable("dbo.Homework");
            DropTable("dbo.CourseIntance");
        }
    }
}
