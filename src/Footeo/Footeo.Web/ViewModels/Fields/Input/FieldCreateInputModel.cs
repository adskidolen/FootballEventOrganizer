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
        public string Town { get; set; }
    }
}