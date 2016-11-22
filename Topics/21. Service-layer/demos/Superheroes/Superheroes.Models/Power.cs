using System.Collections.Generic;

namespace Superheroes.Models
{
    public class Power
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Superhero> Superheros { get; set; }
    }
}