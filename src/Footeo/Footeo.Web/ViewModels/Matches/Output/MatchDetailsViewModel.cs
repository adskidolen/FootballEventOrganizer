﻿namespace Footeo.Web.ViewModels.Matches.Output
{
    public class MatchDetailsViewModel
    {
        private const string None = "N/A";

        public int Id { get; set; }
        public string FieldName { get; set; }

        public string RefereeFullName { get; set; }
        public string ShowReferee => string.IsNullOrWhiteSpace(this.RefereeFullName) ? None : this.RefereeFullName;

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string ShowRivals => $"{this.HomeTeamName} vs {AwayTeamName}";

        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }

        public string Result { get; set; }
        public string ShowResult => string.IsNullOrWhiteSpace(this.Result) ? None : $"{this.HomeTeamGoals} : {this.AwayTeamGoals}";
    }
}