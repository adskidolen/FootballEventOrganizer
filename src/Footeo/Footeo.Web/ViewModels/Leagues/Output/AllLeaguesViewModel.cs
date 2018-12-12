namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System.Collections.Generic;

    public class AllLeaguesViewModel
    {
        public IEnumerable<LeagueViewModel> Leagues { get; set; }
    }
}