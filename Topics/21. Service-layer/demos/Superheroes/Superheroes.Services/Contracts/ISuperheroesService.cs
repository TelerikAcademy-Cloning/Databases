using System.Collections.Generic;
using System.Linq;
using Superheroes.Models;

namespace Superheroes.Services.Contracts
{
    public interface ISuperheroesService
    {
        Superhero CreateSuperhero(
            string superheroName,
            string secretIdentity,
            string cityName,
            IEnumerable<string> powerNames);

        Superhero CreateSuperhero(
            string superheroName,
            string secretIdentity,
            string cityName,
            params string[] powerNames);

        void AddPowerToSuperhero(Superhero superhero, string power);

        IQueryable<Superhero> GetAllSuperheroes();

        IQueryable<Superhero> GetSupeheroesWithPower(string powerName);
    }
}