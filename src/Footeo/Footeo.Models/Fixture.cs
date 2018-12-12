namespace Footeo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Fixture
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

        public Fixture()
        {
            this.Matches = new List<Match>();
        }
    }
}