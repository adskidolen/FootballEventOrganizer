namespace Footeo.Web.Areas.Player.ViewModels.Teams.Input
{
    using System.ComponentModel.DataAnnotations;

    public class TeamCreateInputModel
    {
        private const int MaxNameLength = 30;
        private const int MinNameLength = 1;

        private const int MaxInitialsLength = 5;
        private const int MinInitialsLength = 1;

        [Required]
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxInitialsLength, MinimumLength = MinInitialsLength)]
        public string Initials { get; set; }
    }
}