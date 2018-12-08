namespace Footeo.Web.ViewModels.Teams.View
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeamViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Initials { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        public string TownName { get; set; }
    }
}