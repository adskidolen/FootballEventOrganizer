namespace Footeo.Web.ViewModels.Leagues.Output
{
    using Footeo.Web.ViewModels.TeamLeagues.Output;

    using System.Collections.Generic;

    public class LeagueTableViewModel
    {
        public IEnumerable<TeamLeagueViewModel> Teams { get; set; }
    }
}