namespace Footeo.Web.ViewModels.Home
{
    using Footeo.Web.ViewModels.Leagues.Output;

    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<LeagueIndexViewModel> Leagues { get; set; }
    }
}