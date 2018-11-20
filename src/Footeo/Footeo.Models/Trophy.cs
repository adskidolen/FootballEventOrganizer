namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Trophy
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<LeagueTrophy> Leagues { get; set; }

        public Trophy()
        {
            this.Leagues = new List<LeagueTrophy>();
        }
    }
}