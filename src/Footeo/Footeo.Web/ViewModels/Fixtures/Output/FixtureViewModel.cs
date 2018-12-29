namespace Footeo.Web.ViewModels.Fixtures.Output
{
    using Footeo.Web.ViewModels.Matches.Output;

    using System;
    using System.Collections.Generic;

    public class FixtureViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string ShowInfo => $"{this.Name} - {this.Date}";

        public IEnumerable<MatchFixtureViewModel> Matches { get; set; }
    }
}