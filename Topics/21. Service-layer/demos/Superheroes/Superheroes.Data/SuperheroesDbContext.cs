using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using Superheroes.Models;

namespace Superheroes.Data
{
    public class SuperheroesDbContext : DbContext
    {
        public SuperheroesDbContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<Superhero> Superheros { get; set; }
        public DbSet<Power> Powers { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Superhero>()
                .HasKey(x => x.Id);


            modelBuilder
                .Entity<Superhero>()
                .Property(t => t.SecretIdentity)
                .HasMaxLength(20)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_SecretIdentity", 1) { IsUnique = true }));

            modelBuilder.Entity<Superhero>()
                .HasRequired(x => x.City)
                .WithMany(x => x.Superheros);

            modelBuilder.Entity<Power>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<Power>()
                .Property(t => t.Name)
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Name", 1) { IsUnique = true }));

            modelBuilder.Entity<City>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<City>()
                .Property(t => t.Name)
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Name", 1) { IsUnique = true }));
        }
    }
}
