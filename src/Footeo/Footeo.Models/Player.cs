namespace Footeo.Models
{
    using Footeo.Models.Enums;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Player
    {
        private const int MaxSquadNumberValue = 99;
        private const int MinSquadNumberValue = 1;

        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Nickname { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public bool IsCaptain { get; set; }

        public PlayerPosition? Position { get; set; }

        [Range(MinSquadNumberValue, MaxSquadNumberValue)]
        public int? SquadNumber { get; set; }

        public Player()
        {
            this.IsCaptain = false;
        }
    }
}