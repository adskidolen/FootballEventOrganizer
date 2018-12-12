namespace Footeo.Web.ViewModels.Fixtures.Output
{
    using Footeo.Web.ViewModels.Matches.Output;

    using System;
    using System.Collections.Generic;

    public class FixturesViewModel
    {
        public DateTime Date { get; set; }
        public IEnumerable<MatchViewModel> Matches { get; set; }
    }
}