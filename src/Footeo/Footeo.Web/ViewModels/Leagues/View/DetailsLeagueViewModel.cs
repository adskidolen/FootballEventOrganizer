namespace Footeo.Web.ViewModels.Leagues.View
{
    using Footeo.Web.ViewModels.TeamLeagues.View;

    using System.Collections.Generic;

    public class DetailsLeagueViewModel
    {
        public IEnumerable<TeamLeagueViewModel> Teams { get; set; }
    }
}