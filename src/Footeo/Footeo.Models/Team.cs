namespace Footeo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team
    {
        private const int MaxNameLength = 30;
        private const int MinNameLength = 1;

        private const int MaxInitialsLength = 5;
        private const int MinInitialsLength = 1;

        public int Id { get; set; }

        [Required]
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxInitialsLength, MinimumLength = MinInitialsLength)]
        public string Initials { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<TeamLeague> Leagues { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Trophy> Trophies { get; set; }
        public virtual ICollection<Match> HomeMatches { get; set; }
        public virtual ICollection<Match> AwayMatches { get; set; }

        public Team()
        {
            this.CreatedOn = DateTime.UtcNow;

            this.Leagues = new List<TeamLeague>();
            this.Players = new List<Player>();
            this.Trophies = new List<Trophy>();
            this.HomeMatches = new List<Match>();
            this.AwayMatches = new List<Match>();
        }
    }
}