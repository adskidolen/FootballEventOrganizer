namespace Footeo.Web.Areas.Admin.ViewModels.Leagues.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LeagueCreateInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Town { get; set; }
    }
}