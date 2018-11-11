namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Leg
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

        public Leg()
        {
            this.Matches = new List<Match>();
        }
    }
}