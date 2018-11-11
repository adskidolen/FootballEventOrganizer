namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Field
    {
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool IsIndoors { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

        public Field()
        {
            this.Matches = new List<Match>();
        }
    }
}