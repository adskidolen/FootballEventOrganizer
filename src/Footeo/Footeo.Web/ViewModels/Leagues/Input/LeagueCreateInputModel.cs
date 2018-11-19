namespace Footeo.Web.ViewModels.Leagues.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LeagueCreateInputModel
    {
        // TODO: Validation for LeagueCreateInputModel

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
        public string Town { get; set; }
    }
}