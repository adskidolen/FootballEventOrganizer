namespace Footeo.Web.ViewModels.Teams.Output
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeamViewModel
    {
        private const string CreatedOnDisplayName = "Created On";

        public int Id { get; set; }

        public string Name { get; set; }

        public string Initials { get; set; }

        [Display(Name = CreatedOnDisplayName)]
        public DateTime CreatedOn { get; set; }

        public string TownName { get; set; }

        public int TrophiesCount { get; set; }
    }
}