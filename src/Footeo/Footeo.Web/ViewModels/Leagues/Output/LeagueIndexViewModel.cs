namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System;

    public class LeagueIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string ShowTitle => $"{this.Name} - {this.StartDate.ToShortDateString()}";

        public string Description { get; set; }
    }
}