namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Town
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<FooteoUser> FooteoUsers { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<League> Leagues { get; set; }

        public Town()
        {
            this.FooteoUsers = new List<FooteoUser>();
            this.Teams = new List<Team>();
            this.Fields = new List<Field>();
            this.Leagues = new List<League>();
        }
    }
}