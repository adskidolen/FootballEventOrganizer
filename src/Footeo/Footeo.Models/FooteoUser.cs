namespace Footeo.Models
{
    using Microsoft.AspNetCore.Identity;

    public class FooteoUser : IdentityUser
    {
        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int? RefereeId { get; set; }
        public virtual Referee Referee { get; set; }
    }
}