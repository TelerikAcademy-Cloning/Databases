namespace Superheroes.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedcitiesofsuperherotoasinglecity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SuperheroCities", "Superhero_Id", "dbo.Superheroes");
            DropForeignKey("dbo.SuperheroCities", "City_Id", "dbo.Cities");
            DropIndex("dbo.SuperheroCities", new[] { "Superhero_Id" });
            DropIndex("dbo.SuperheroCities", new[] { "City_Id" });
            AddColumn("dbo.Superheroes", "City_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Superheroes", "City_Id");
            AddForeignKey("dbo.Superheroes", "City_Id", "dbo.Cities", "Id", cascadeDelete: true);
            DropTable("dbo.SuperheroCities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SuperheroCities",
                c => new
                    {
                        Superhero_Id = c.Int(nullable: false),
                        City_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Superhero_Id, t.City_Id });
            
            DropForeignKey("dbo.Superheroes", "City_Id", "dbo.Cities");
            DropIndex("dbo.Superheroes", new[] { "City_Id" });
            DropColumn("dbo.Superheroes", "City_Id");
            CreateIndex("dbo.SuperheroCities", "City_Id");
            CreateIndex("dbo.SuperheroCities", "Superhero_Id");
            AddForeignKey("dbo.SuperheroCities", "City_Id", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SuperheroCities", "Superhero_Id", "dbo.Superheroes", "Id", cascadeDelete: true);
        }
    }
}
