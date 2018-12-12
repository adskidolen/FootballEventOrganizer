using System.ComponentModel.DataAnnotations;

namespace Footeo.Web.ViewModels.Teams.Input
{
    public class TeamCreateInputModel
    {
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        public string Initials { get; set; }

        //public byte[] Logo { get; set; }
    }
}