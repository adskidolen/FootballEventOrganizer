namespace Footeo.Web.ViewModels.Fixtures.Output
{
    using Footeo.Web.ViewModels.Matches.Output;

    using System.Collections.Generic;

    public class FixtureDetailsViewModel
    {
        public IEnumerable<MatchViewModel> Matches { get; set; }
    }
}