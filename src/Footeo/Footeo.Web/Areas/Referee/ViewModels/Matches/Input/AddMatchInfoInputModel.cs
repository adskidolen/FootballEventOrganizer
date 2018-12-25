namespace Footeo.Web.Areas.Referee.ViewModels.Matches.Input
{
    using Footeo.Web.Areas.Referee.ViewModels.Players.Input;

    using System.Collections.Generic;
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

        public IList<PlayerStatisticsInputModel> Stats { get; set; }

        public AddMatchInfoInputModel()
        {
            this.Stats = new List<PlayerStatisticsInputModel>();
        }
    }
}