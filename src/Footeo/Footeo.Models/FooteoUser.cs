namespace Footeo.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FooteoUser : IdentityUser
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [Range(14, 50)]
        public int Age { get; set; }

        //[Required]
        //public byte[] Picture { get; set; }

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int? RefereeId { get; set; }
        public virtual Referee Referee { get; set; }
    }
}