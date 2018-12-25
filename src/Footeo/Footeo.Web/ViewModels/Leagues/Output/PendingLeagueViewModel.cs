namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System;

    public class PendingLeagueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TownName { get; set; }
    }
}