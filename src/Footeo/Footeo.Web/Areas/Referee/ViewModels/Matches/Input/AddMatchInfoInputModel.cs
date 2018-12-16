namespace Footeo.Web.Areas.Referee.ViewModels.Matches.Input
{
    using System.ComponentModel.DataAnnotations;

    public class AddMatchInfoInputModel
    {
        [Required]
        [Range(0, 30)]
        public int HomeTeamGoals { get; set; }

        [Required]
        [Range(0, 30)]
        public int AwayTeamGoals { get; set; }
    }
}