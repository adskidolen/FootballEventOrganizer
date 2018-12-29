namespace Footeo.Web.ViewModels.Leagues.Output
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InProgressLeagueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string ShowEndDate => this.EndDate.ToShortDateString();

        public string TownName { get; set; }
    }
}