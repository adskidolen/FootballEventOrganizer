namespace Footeo.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TeamLeague
    {
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        [Range(0, 150)]
        public int Points { get; set; }

        // TODO: Edit position
        [Range(1, 20)]
        public int Position { get; set; }

        [Range(0, int.MaxValue)]
        public int GoalsFor { get; set; }

        [Range(0, int.MaxValue)]
        public int GoalsAgainst { get; set; }

        public int GoalDifference { get; set; }

        [Range(0, int.MaxValue)]
        public int PlayedMatches { get; set; }

        [Range(0, int.MaxValue)]
        public int Won { get; set; }

        [Range(0, int.MaxValue)]
        public int Drawn { get; set; }

        [Range(0, int.MaxValue)]
        public int Lost { get; set; }

        public TeamLeague()
        {
            this.Points = 0;
            this.GoalsFor = 0;
            this.GoalsAgainst = 0;
            this.GoalDifference = 0;
            this.PlayedMatches = 0;
            this.Won = 0;
            this.Drawn = 0;
            this.Lost = 0;
        }
    }
}