namespace Footeo.Web.ViewModels.Fixtures.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FixtureCreateInputModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int LeagueId { get; set; }
    }
}