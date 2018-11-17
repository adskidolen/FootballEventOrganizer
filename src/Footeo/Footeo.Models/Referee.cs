namespace Footeo.Models
{
    using System.Collections.Generic;

    public class Referee
    {
        public int Id { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

        public Referee()
        {
            this.Matches = new List<Match>();
        }
    }
}