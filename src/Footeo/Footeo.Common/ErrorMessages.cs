namespace Footeo.Common
{
    public static class ErrorMessages
    {
        public const string PlayerInTeamErrorMessage = "Player {0} already has a team!";

        public const string TeamExistsErrorMessage = "Team {0} already exists!";
        public const string TeamDoesNotExistsErrorMessage = "Team does not exists!";
        public const string TeamIsFullErrorMessage = "Team with id {0} already full!";
        public const string TeamJoinedLeagueErrorMessage = "Team {0} already joined a league!";

        public const string LeagueDoesNotExistsErrorMessage = "League does not exists!";
        public const string LeagueExistsErrorMessage = "League {0} already exists!";

        public const string FieldExistsErrorMessage = "Field {0} already exists!";

        public const string MatchDoesNotExistsErrorMessage = "Match does not exists!";
        public const string MatchHasRefereeErrorMessage = "The Match already has a referee!";
        public const string MatchHasResultErrorMessage = "The Match already has a result!";
    }
}