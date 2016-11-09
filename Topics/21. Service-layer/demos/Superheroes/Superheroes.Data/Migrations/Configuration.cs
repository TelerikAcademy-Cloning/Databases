using Superheroes.Models;

namespace Superheroes.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Superheroes.Data.SuperheroesDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }



        protected override void Seed(Superheroes.Data.SuperheroesDbContext context)
        {
            var power = new Power
            {
                Name = "Flying"
            };

            context.Powers.AddOrUpdate(x => x.Name, power);

            context.SaveChanges();
        }

    }
}
