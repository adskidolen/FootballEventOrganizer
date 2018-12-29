namespace Footeo.Web.ViewModels.TeamLeagues.Output
{
    public class TeamLeagueViewModel
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int Points { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int GoalDifference => this.GoalsFor - this.GoalsAgainst;

        public int PlayedMatches { get; set; }

        public int Won { get; set; }

        public int Drawn { get; set; }

        public int Lost { get; set; }
    }
}
