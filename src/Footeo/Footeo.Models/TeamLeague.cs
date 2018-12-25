namespace Footeo.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TeamLeague
    {
        private const int MaxPointsValue = 150;
        private const int MinPointsValue = 0;

        private const int HighestPositionVaue = 1;
        private const int LowestPositionValue = 10;

        private const int MinValue = 0;

        [Required]
        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        [Required]
        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        [Range(MinPointsValue, MaxPointsValue)]
        public int Points { get; set; }
        
        [Range(HighestPositionVaue, LowestPositionValue)]
        public int Position { get; set; }

        [Range(MinValue, int.MaxValue)]
        public int GoalsFor { get; set; }

        [Range(MinValue, int.MaxValue)]
        public int GoalsAgainst { get; set; }

        public int GoalDifference { get; set; }

        [Range(MinValue, int.MaxValue)]
        public int PlayedMatches { get; set; }

        [Range(MinValue, int.MaxValue)]
        public int Won { get; set; }

        [Range(MinValue, int.MaxValue)]
        public int Drawn { get; set; }

        [Range(MinValue, int.MaxValue)]
        public int Lost { get; set; }

        public TeamLeague()
        {
            this.Points = MinValue;
            this.GoalsFor = MinValue;
            this.GoalsAgainst = MinValue;
            this.GoalDifference = MinValue;
            this.PlayedMatches = MinValue;
            this.Won = MinValue;
            this.Drawn = MinValue;
            this.Lost = MinValue;
        }
    }
}