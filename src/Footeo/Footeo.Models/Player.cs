namespace Footeo.Models
{
    using Footeo.Models.Enums;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Player
    {
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 1)]
        public string Nickname { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        [Range(1, 99)]
        public int SquadNumber { get; set; }

        [Range(0, 10)]
        public int Rating { get; set; }

        //[Range(typeof(decimal), "0", "79228162514264337593543950335")]
        //public double Height { get; set; }

        //[Range(typeof(decimal), "0", "79228162514264337593543950335")]
        //public double Weight { get; set; }

        public virtual ICollection<PlayerStatistic> Statistics { get; set; }

        public Player()
        {
            this.Rating = 0;
            this.Statistics = new List<PlayerStatistic>();
        }
    }
}