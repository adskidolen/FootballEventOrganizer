namespace Footeo.Models
{
    public class PlayerStatistic
    {
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int MatchId { get; set; }
        public virtual Match Match { get; set; }

        public int GoalsScored { get; set; }
        public int Assists { get; set; }
    }
}