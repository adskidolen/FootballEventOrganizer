namespace Footeo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Match
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("HomeTeam")]
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }

        [Range(0, 30)]
        public int? HomeTeamGoals { get; set; }

        [Required]
        [ForeignKey("AwayTeam")]
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }

        [Range(0, 30)]
        public int? AwayTeamGoals { get; set; }

        public string Result { get; set; }

        [Required]
        [ForeignKey(nameof(Fixture))]
        public int FixtureId { get; set; }
        public virtual Fixture Fixture { get; set; }

        [ForeignKey(nameof(Referee))]
        public int? RefereeId { get; set; }
        public virtual Referee Referee { get; set; }

        [Required]
        [ForeignKey(nameof(Field))]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }

        public virtual ICollection<PlayerStatistic> Statistics { get; set; }

        public Match()
        {
            this.Statistics = new List<PlayerStatistic>();
        }
    }
}