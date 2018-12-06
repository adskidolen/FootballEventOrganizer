namespace Footeo.Web.ViewModels.Leagues.View
{
    using Footeo.Models.Enums;

    using System;

    public class LeagueViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Status Status { get; set; }

        public string TownName { get; set; }
    }
}