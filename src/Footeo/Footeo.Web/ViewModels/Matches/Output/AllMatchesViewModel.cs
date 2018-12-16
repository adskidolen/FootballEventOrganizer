namespace Footeo.Web.ViewModels.Matches.Output
{
    using System.Collections.Generic;

    public class AllMatchesViewModel
    {
        public IEnumerable<MatchDetailsViewModel> Matches { get; set; }
    }
}