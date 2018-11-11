namespace Footeo.Models
{
    using Footeo.Models.Enums;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Player
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string Nickname { get; set; }

        [Required]
        [Range(14, 50, ErrorMessage = "Player must be over 14 years old and under 50 years old!")]
        public int Age { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        [Required]
        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        public int SquadNumber { get; set; }

        [Range(0, 10)]
        public int Rating { get; set; }

        public double Height { get; set; }
        public double Weight { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        public virtual ICollection<PlayerStatistic> Statistics { get; set; }

        public Player()
        {
            this.Statistics = new List<PlayerStatistic>();
        }
    }
}