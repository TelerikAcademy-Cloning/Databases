namespace Superheroes.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Addedconstaints : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Index_Unique_CityName",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { IsUnique: True }")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Superheroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SuperheroName = c.String(),
                        SecretIdentity = c.String(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Index_Unique_SecretIdentity",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { IsUnique: True }")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Powers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Index_Unique_PowerName",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { IsUnique: True }")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperheroCities",
                c => new
                    {
                        Superhero_Id = c.Int(nullable: false),
                        City_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Superhero_Id, t.City_Id })
                .ForeignKey("dbo.Superheroes", t => t.Superhero_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cities", t => t.City_Id, cascadeDelete: true)
                .Index(t => t.Superhero_Id)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.PowerSuperheroes",
                c => new
                    {
                        Power_Id = c.Int(nullable: false),
                        Superhero_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Power_Id, t.Superhero_Id })
                .ForeignKey("dbo.Powers", t => t.Power_Id, cascadeDelete: true)
                .ForeignKey("dbo.Superheroes", t => t.Superhero_Id, cascadeDelete: true)
                .Index(t => t.Power_Id)
                .Index(t => t.Superhero_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PowerSuperheroes", "Superhero_Id", "dbo.Superheroes");
            DropForeignKey("dbo.PowerSuperheroes", "Power_Id", "dbo.Powers");
            DropForeignKey("dbo.SuperheroCities", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.SuperheroCities", "Superhero_Id", "dbo.Superheroes");
            DropIndex("dbo.PowerSuperheroes", new[] { "Superhero_Id" });
            DropIndex("dbo.PowerSuperheroes", new[] { "Power_Id" });
            DropIndex("dbo.SuperheroCities", new[] { "City_Id" });
            DropIndex("dbo.SuperheroCities", new[] { "Superhero_Id" });
            DropTable("dbo.PowerSuperheroes");
            DropTable("dbo.SuperheroCities");
            DropTable("dbo.Powers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Name",
                        new Dictionary<string, object>
                        {
                            { "Index_Unique_PowerName", "IndexAnnotation: { IsUnique: True }" },
                        }
                    },
                });
            DropTable("dbo.Superheroes",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "SecretIdentity",
                        new Dictionary<string, object>
                        {
                            { "Index_Unique_SecretIdentity", "IndexAnnotation: { IsUnique: True }" },
                        }
                    },
                });
            DropTable("dbo.Cities",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Name",
                        new Dictionary<string, object>
                        {
                            { "Index_Unique_CityName", "IndexAnnotation: { IsUnique: True }" },
                        }
                    },
                });
        }
    }
}
