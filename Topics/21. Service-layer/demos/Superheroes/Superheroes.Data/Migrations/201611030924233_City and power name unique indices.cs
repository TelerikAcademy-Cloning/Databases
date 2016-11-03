namespace Superheroes.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Cityandpowernameuniqueindices : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String(maxLength: 60,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Index_Unique_CityName",
                        new AnnotationValues(oldValue: "IndexAnnotation: { IsUnique: True }", newValue: null)
                    },
                }));
            AlterColumn("dbo.Powers", "Name", c => c.String(maxLength: 60,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Index_Unique_PowerName",
                        new AnnotationValues(oldValue: "IndexAnnotation: { IsUnique: True }", newValue: null)
                    },
                }));
            CreateIndex("dbo.Cities", "Name", unique: true);
            CreateIndex("dbo.Powers", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Powers", new[] { "Name" });
            DropIndex("dbo.Cities", new[] { "Name" });
            AlterColumn("dbo.Powers", "Name", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Index_Unique_PowerName",
                        new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { IsUnique: True }")
                    },
                }));
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Index_Unique_CityName",
                        new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { IsUnique: True }")
                    },
                }));
        }
    }
}
