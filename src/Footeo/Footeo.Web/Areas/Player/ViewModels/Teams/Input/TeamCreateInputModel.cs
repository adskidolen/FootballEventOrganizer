namespace Footeo.Web.Areas.Player.ViewModels.Teams.Input
{
    using System.ComponentModel.DataAnnotations;

    public class TeamCreateInputModel
    {
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        public string Initials { get; set; }
    }
}