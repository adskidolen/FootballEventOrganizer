namespace Footeo.Web.Areas.Admin.ViewModels.Fixtures.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FixtureCreateInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int LeagueId { get; set; }
    }
}