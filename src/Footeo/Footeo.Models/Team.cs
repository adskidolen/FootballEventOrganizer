namespace Footeo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Initials { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        [Range(0, 150)]
        public int Points { get; set; }

        [Required]
        [Range(0, 20)]
        public int Position { get; set; }

        [Required]
        public int GoalsFor { get; set; }

        [Required]
        public int GoalsAgainst { get; set; }

        [Required]
        public int GoalDifference { get; set; }

        [Required]
        public int PlayedMatches { get; set; }

        [Required]
        public int Won { get; set; }

        [Required]
        public int Drawn { get; set; }

        [Required]
        public int Lost { get; set; }

        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Trophy> Trophies { get; set; }
        public virtual ICollection<Match> HomeMatches { get; set; }
        public virtual ICollection<Match> AwayMatches { get; set; }

        public Team()
        {
            this.CreatedOn = DateTime.UtcNow;

            this.Players = new List<Player>();
            this.Trophies = new List<Trophy>();
            this.HomeMatches = new List<Match>();
            this.AwayMatches = new List<Match>();
        }
    }
}