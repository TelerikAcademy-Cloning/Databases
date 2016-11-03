using System.Collections.Generic;

namespace Superheroes.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Superhero> Superheros { get; set; }
    }
}