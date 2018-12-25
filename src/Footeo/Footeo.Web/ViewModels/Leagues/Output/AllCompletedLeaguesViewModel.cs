namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System.Collections.Generic;

    public class AllCompletedLeaguesViewModel
    {
        public IEnumerable<CompletedLeagueViewModel> Leagues { get; set; }
    }
}