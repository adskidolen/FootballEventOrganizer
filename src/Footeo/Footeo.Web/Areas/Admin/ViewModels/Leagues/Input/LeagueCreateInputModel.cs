namespace Footeo.Web.Areas.Admin.ViewModels.Leagues.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LeagueCreateInputModel
    {
        private const int NameMaxLength = 100;
        private const int NameMinLength = 3;

        private const int TownMaxLength = 30;
        private const int TownMinLength = 3;

        private const string StartDateDisplayName = "Date of Start";
        private const string EndDateDisplayName = "Date of End";

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = StartDateDisplayName)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = EndDateDisplayName)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength)]
        public string Town { get; set; }
    }
}