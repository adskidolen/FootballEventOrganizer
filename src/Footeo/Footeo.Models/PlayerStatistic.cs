namespace Footeo.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PlayerStatistic
    {
        [Required]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [Required]
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }

        [Required]
        [Range(0, 20)]
        public int GoalsScored { get; set; }

        [Required]
        [Range(0, 20)]
        public int Assists { get; set; }

        public PlayerStatistic()
        {
            this.GoalsScored = 0;
            this.Assists = 0;
        }
    }
}