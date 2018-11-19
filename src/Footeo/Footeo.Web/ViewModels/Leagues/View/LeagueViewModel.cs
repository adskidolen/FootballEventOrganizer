namespace Footeo.Web.ViewModels.Leagues.View
{
    using Footeo.Models.Enums;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class LeagueViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public string Town { get; set; }
    }
}