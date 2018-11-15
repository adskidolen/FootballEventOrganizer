namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class League
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string News { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<TeamLeague> Teams { get; set; }
        public virtual ICollection<Fixture> Fixtures { get; set; }

        public League()
        {
            this.Teams = new List<TeamLeague>();
            this.Fixtures = new List<Fixture>();
        }
    }
}