namespace Footeo.Web.ViewModels.Matches.Output
{
    public class MatchDetailsViewModel
    {
        public int Id { get; set; }
        public string RefereeFullName { get; set; }
        public string FieldName { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public string Result => $"{this.HomeTeamGoals} : {this.AwayTeamGoals}";
    }
}