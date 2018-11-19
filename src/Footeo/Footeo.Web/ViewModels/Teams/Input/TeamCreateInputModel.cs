using System.ComponentModel.DataAnnotations;

namespace Footeo.Web.ViewModels.Teams.Input
{
    public class TeamCreateInputModel
    {
        // TODO: Validation

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        public string Initials { get; set; }

        //public byte[] Logo { get; set; }

        [Required]
        public string Town { get; set; }
    }
}