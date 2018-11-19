namespace Footeo.Web.ViewModels.Teams.View
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeamViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        public string Initials { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string Town { get; set; }
    }
}