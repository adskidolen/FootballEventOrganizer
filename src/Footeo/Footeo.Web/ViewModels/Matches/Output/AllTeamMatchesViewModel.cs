namespace Footeo.Web.ViewModels.Matches.Output
{
    using System.Collections.Generic;

    public class AllTeamMatchesViewModel
    {
        public IEnumerable<TeamMatchViewModel> HomeMatches { get; set; }
        public IEnumerable<TeamMatchViewModel> AwayMatches { get; set; }
    }
}