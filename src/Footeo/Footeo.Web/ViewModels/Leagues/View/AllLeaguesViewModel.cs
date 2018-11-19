namespace Footeo.Web.ViewModels.Leagues.View
{
    using System.Collections.Generic;

    public class AllLeaguesViewModel
    {
        public IEnumerable<LeagueViewModel> Leagues { get; set; }
    }
}