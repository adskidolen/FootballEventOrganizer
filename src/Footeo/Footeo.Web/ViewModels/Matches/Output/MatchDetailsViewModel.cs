namespace Footeo.Web.ViewModels.Matches.Output
{
    public class MatchDetailsViewModel
    {
        public int Id { get; set; }
        public string RefereeFullName { get; set; }
        public string FieldName { get; set; }

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string ShowRivals => $"{this.HomeTeamName} vs {AwayTeamName}";

        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public string Result => $"{this.HomeTeamGoals} : {this.AwayTeamGoals}";
    }
}