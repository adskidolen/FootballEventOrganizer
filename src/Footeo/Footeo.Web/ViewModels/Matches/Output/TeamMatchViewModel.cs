namespace Footeo.Web.ViewModels.Matches.Output
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeamMatchViewModel
    {
        public string FixtureName { get; set; }

        [DataType(DataType.Time)]
        public DateTime Date { get; set; }
        public string ShowDate => this.Date.ToShortDateString();

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string ShowRivals => $"{this.HomeTeamName} vs {this.AwayTeamName}";

        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public string ShowResult => $"{this.HomeTeamGoals} : {this.AwayTeamGoals}";
    }
}