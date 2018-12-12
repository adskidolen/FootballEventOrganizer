namespace Footeo.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PlayerStatistic
    {
        [Required]
        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [Required]
        [ForeignKey(nameof(Match))]
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