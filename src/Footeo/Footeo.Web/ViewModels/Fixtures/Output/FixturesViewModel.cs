namespace Footeo.Web.ViewModels.Fixtures.Output
{
    using Footeo.Web.ViewModels.Matches.Output;

    using System;
    using System.Collections.Generic;

    public class FixturesViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Info => $"{this.Name} - {this.Date}";
        public IEnumerable<MatchViewModel> Matches { get; set; }
    }
}