namespace Footeo.Web.ViewModels.Matches.Output
{
    public class MatchViewModel
    {
        public int Id { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string Rivals => $"{this.HomeTeamName} vs {this.AwayTeamName}";
    }
}