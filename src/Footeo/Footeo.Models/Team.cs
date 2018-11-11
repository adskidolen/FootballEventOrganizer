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
        [Range(0, int.MaxValue)]
        public int GoalsFor { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int GoalsAgainst { get; set; }

        [Required]
        public int GoalDifference { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int PlayedMatches { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Won { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Drawn { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Lost { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        [Required]
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