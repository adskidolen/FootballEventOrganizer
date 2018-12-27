namespace Footeo.Web.ViewModels.Teams.Output
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeamDetailsViewModel
    {
        private const string CreatedOnDisplayName = "Date of Creation";

        public int Id { get; set; }

        public string Name { get; set; }
        public string Initials { get; set; }
        public string ShowTeamName => $"{this.Name} ({this.Initials})";

        [Display(Name = CreatedOnDisplayName)]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        public string ShowDateOfCreation => this.CreatedOn.ToShortDateString();

        public int TrophiesCount { get; set; }
    }
}