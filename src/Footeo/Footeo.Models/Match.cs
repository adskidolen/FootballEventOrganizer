namespace Footeo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Match
    {
        private const string HomeTeamProperyName = "HomeTeam";
        private const string AwayTeamProperyName = "AwayTeam";

        private const int GoalsMinValue = 0;
        private const int GoalsMaxValue = 30;

        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(HomeTeamProperyName)]
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }

        [Range(GoalsMinValue, GoalsMaxValue)]
        public int? HomeTeamGoals { get; set; }

        [Required]
        [ForeignKey(AwayTeamProperyName)]
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }

        [Range(GoalsMinValue, GoalsMaxValue)]
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
    }
}