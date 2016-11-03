using System;
using System.Collections.Generic;
using System.Linq;
using Superheroes.Data;
using Superheroes.Models;
using Superheroes.Repositories;
using Superheroes.Repositories.Contracts;
using Superheroes.Services.Contracts;

namespace Superheroes.Services
{
    public class SuperheroesService : ISuperheroesService
    {
        private const int MinSuperheroNameLength = 3;
        private const int MaxSuperherNameLength = 20;

        private readonly IUnitOfWork unitOfWork;

        private readonly IRepository<Superhero> superheroes;
        private readonly IRepository<City> cities;
        private readonly IRepository<Power> powers;

        public SuperheroesService(
            IUnitOfWork unitOfWork,
            IRepository<Superhero> superheroes,
            IRepository<Power> powers,
            IRepository<City> cities)
        {
            this.unitOfWork = unitOfWork;
            this.superheroes = superheroes;
            this.powers = powers;
            this.cities = cities;
        }

        public Superhero CreateSuperhero(string superheroName, string secretIdentity, string cityName, IEnumerable<string> powerNames)
        {
            if (string.IsNullOrEmpty(superheroName) ||
                superheroName.Length < MinSuperheroNameLength ||
            superheroName.Length > MaxSuperherNameLength)
            {
                throw new ArgumentException("Invalid supehero name");
            }

            var superhero = new Superhero
            {
                SuperheroName = superheroName,
                SecretIdentity = secretIdentity,
                City = this.LoadOrCreateCity(cityName),
                Powers = powerNames?.Select(this.LoadOrCreatePower)
                    .ToList()
            };

            using (this.unitOfWork)
            {
                this.superheroes.Add(superhero);
                this.unitOfWork.Commit();
            }

            return superhero;
        }

        public Superhero CreateSuperhero(string superheroName, string secretIdentity, string cityName, params string[] powerNames)
        {
            return this.CreateSuperhero(superheroName, secretIdentity, cityName, new List<string>(powerNames));
        }

        public void AddPowerToSuperhero(Superhero superhero, string powerName)
        {
            var dbSuperhero = this.superheroes.GetById(superhero.Id);
            if (dbSuperhero == null)
            {
                throw new ArgumentNullException(nameof(superhero), "Invalid superhero");
            }

            using (unitOfWork)
            {
                dbSuperhero.Powers.Add(this.LoadOrCreatePower(powerName));
                unitOfWork.Commit();
            }
        }

        public IQueryable<Superhero> GetAllSuperheroes()
        {
            return this.superheroes.Entities;
        }

        public IQueryable<Superhero> GetSupeheroesWithPower(string powerName)
        {
            var power = this.powers.Entities
                .FirstOrDefault(p => p.Name == powerName);

            if (power != null)
                return power
                    .Superheros
                    .AsQueryable();

            return new List<Superhero>()
                .AsQueryable();
        }

        private Power LoadOrCreatePower(string powerName)
        {
            var city = this.powers
                   .Entities.FirstOrDefault(c => c.Name == powerName);

            return city ?? new Power
            {
                Name = powerName
            };
        }

        private City LoadOrCreateCity(string cityName)
        {
            var city = this.cities
                .Entities.FirstOrDefault(c => c.Name == cityName);

            return city ?? new City
            {
                Name = cityName
            };
        }

    }
}
