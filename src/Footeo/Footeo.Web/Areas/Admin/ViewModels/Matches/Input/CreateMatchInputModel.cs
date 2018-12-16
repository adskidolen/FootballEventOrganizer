namespace Footeo.Web.Areas.Admin.ViewModels.Matches.Input
{
    using System.ComponentModel.DataAnnotations;

    public class CreateMatchInputModel
    {
        [Required]
        public int HomeTeamId { get; set; }

        [Required]
        public int AwayTeamId { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        public int FixtureId { get; set; }
    }
}