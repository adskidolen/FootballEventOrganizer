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
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }

        public int HomeTeamGoals { get; set; }

        [Required]
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }

        public int AwayTeamGoals { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 3)]
        public string Result { get; set; }

        [ForeignKey(nameof(Referee))]
        public int RefereeId { get; set; }
        public virtual Referee Referee { get; set; }

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