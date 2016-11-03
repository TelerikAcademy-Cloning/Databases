using System.Collections.Generic;

namespace Superheroes.Models
{
    public class Superhero
    {
        public int Id { get; set; }

        public string SuperheroName { get; set; }

        public string SecretIdentity { get; set; }

        public virtual ICollection<Power> Powers { get; set; }

        public virtual City City { get; set; }

        public override string ToString()
        {
            return $"Id: {this.Id}, Name: {this.SuperheroName}";
        }
    }
}
