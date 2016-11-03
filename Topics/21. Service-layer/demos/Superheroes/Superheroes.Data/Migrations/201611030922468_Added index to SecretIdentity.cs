namespace Superheroes.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedindextoSecretIdentity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Superheroes", "SecretIdentity", c => c.String(maxLength: 20,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Index_Unique_SecretIdentity",
                        new AnnotationValues(oldValue: "IndexAnnotation: { IsUnique: True }", newValue: null)
                    },
                }));
            CreateIndex("dbo.Superheroes", "SecretIdentity", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Superheroes", new[] { "SecretIdentity" });
            AlterColumn("dbo.Superheroes", "SecretIdentity", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Index_Unique_SecretIdentity",
                        new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { IsUnique: True }")
                    },
                }));
        }
    }
}
