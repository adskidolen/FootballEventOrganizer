namespace Footeo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Fixture
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual ICollection<Leg> Legs { get; set; }

        public Fixture()
        {
            this.Legs = new List<Leg>();
        }
    }
}