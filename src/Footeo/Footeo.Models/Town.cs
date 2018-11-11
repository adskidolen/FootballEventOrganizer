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

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Referee> Referees { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<League> Leagues { get; set; }

        public Town()
        {
            this.Players = new List<Player>();
            this.Referees = new List<Referee>();
            this.Teams = new List<Team>();
            this.Fields = new List<Field>();
            this.Leagues = new List<League>();
        }
    }
}