namespace Footeo.Common
{
    public static class ErrorMessages
    {
        public const string PlayerInTeamErrorMessage = "Player {0} already has a team!";

        public const string TeamExistsErrorMessage = "Team {0} already exists!";
        public const string TeamDoesNotExistsErrorMessage = "Team does not exists!";
        public const string TeamIsFullErrorMessage = "Team with id {0} already full!";
        public const string TeamJoinedLeagueErrorMessage = "Team {0} already joined a league!";
        public static string TeamTrophyErrorMessage = "Team already has that trophy!";

        public const string LeagueDoesNotExistsErrorMessage = "League does not exists!";
        public const string LeagueExistsErrorMessage = "League {0} already exists!";
        public const string MaxTeamsInLeagueErrorMessage = "The maximum teams in the leage is 10!";
        public const string NotEnoughTeamsInLeagueErrorMessage = "The league does not reached 10 teams for creating fixtures!";

        public const string FieldExistsErrorMessage = "Field {0} already exists!";

        public const string MatchDoesNotExistsErrorMessage = "Match does not exists!";
        public const string MatchHasRefereeErrorMessage = "The Match already has a referee!";
        public const string MatchHasResultErrorMessage = "The Match already has a result!";

        public const string FixtureDoesNotExistsErrorMessage = "Fixture does not exists!";

        public const string SquadNumberErrorMessage = "Squad Number is taken!";

    }
}