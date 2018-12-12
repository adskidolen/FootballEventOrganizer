namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Referee
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

        public Referee()
        {
            this.Matches = new List<Match>();
        }
    }
}