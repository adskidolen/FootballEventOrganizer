namespace Footeo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Town
    {
        private const int NameMaxLength = 30;
        private const int NameMinLength = 3;

        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
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