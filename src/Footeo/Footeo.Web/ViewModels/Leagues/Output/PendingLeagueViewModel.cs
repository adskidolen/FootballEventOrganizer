namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PendingLeagueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string ShowDuration => $"From {this.StartDate.ToLongDateString()} to {this.EndDate.ToLongDateString()}";

        public string TownName { get; set; }
    }
}