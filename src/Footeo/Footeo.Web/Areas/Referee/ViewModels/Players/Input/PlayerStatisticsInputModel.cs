namespace Footeo.Web.Areas.Referee.ViewModels.Players.Input
{
    using System.ComponentModel.DataAnnotations;

    public class PlayerStatisticsInputModel
    {
        private const int MaxGoalsValue = 20;
        private const int MinGoalsValue = 0;

        private const int MaxAssistsValue = 20;
        private const int MinAssistsValue = 0;

        [Range(MinGoalsValue, MaxGoalsValue)]
        public int GoalsScored { get; set; }

        [Range(MinAssistsValue, MaxAssistsValue)]
        public int Assists { get; set; }
    }
}