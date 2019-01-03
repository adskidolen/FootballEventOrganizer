namespace Footeo.Web.Areas.Referee.ViewModels.Matches.Input
{
    using System.ComponentModel.DataAnnotations;

    public class AddMatchInfoInputModel
    {
        private const int MaxGoalsValue = 30;
        private const int MinGoalsValue = 0;

        [Required]
        [Range(MinGoalsValue, MaxGoalsValue)]
        public int HomeTeamGoals { get; set; }

        [Required]
        [Range(MinGoalsValue, MaxGoalsValue)]
        public int AwayTeamGoals { get; set; }
    }
}