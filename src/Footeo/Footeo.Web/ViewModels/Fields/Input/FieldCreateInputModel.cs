namespace Footeo.Web.ViewModels.Fields.Input
{
    using System.ComponentModel.DataAnnotations;

    public class FieldCreateInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool IsIndoors { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Town { get; set; }
    }
}