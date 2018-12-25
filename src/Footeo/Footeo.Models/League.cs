namespace Footeo.Models
{
    using Footeo.Models.Enums;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class League
    {
        private const int NameMaxLength = 100;
        private const int NameMinLength = 3;

        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public LeagueStatus Status { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public virtual ICollection<TeamLeague> Teams { get; set; }
        public virtual ICollection<Fixture> Fixtures { get; set; }

        public League()
        {
            this.Status = LeagueStatus.Pending;

            this.Teams = new List<TeamLeague>();
            this.Fixtures = new List<Fixture>();
        }
    }
}