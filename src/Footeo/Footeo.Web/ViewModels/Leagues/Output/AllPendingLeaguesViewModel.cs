namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System.Collections.Generic;

    public class AllPendingLeaguesViewModel
    {
        public IEnumerable<PendingLeagueViewModel> Leagues { get; set; }
    }
}