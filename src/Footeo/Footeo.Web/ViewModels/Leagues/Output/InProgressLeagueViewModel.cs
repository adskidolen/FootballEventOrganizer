namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System;

    public class InProgressLeagueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TownName { get; set; }
    }
}