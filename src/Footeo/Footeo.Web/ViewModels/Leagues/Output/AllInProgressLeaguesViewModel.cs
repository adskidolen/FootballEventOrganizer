namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System.Collections.Generic;

    public class AllInProgressLeaguesViewModel
    {
        public IEnumerable<InProgressLeagueViewModel> Leagues { get; set; }
    }
}