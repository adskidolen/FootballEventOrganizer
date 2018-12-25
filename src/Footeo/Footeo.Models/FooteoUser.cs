namespace Footeo.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;

    public class FooteoUser : IdentityUser
    {
        private const int NameMaxLength = 30;
        private const int NameMinLength = 3;

        private const int AgeMaxLength = 50;
        private const int AgeMinLength = 14;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [Required]
        [Range(AgeMinLength, AgeMaxLength)]
        public int Age { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        [ForeignKey(nameof(Player))]
        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey(nameof(Referee))]
        public int? RefereeId { get; set; }
        public virtual Referee Referee { get; set; }
    }
}