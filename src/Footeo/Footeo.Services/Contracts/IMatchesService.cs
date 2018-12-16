namespace Footeo.Services.Contracts
{
    using System.Linq;

    public interface IMatchesService
    {
        void CreateMatch(int homeTeamId, int awayTeamId, int fieldId, int fixtureId);
        TModel GetMatchById<TModel>(int id);
        bool MatchHasReferee(int matchId);
        bool MatchExistsById(int id);
        IQueryable<TModel> MatchesByFixture<TModel>(int fixtureId);
        int GetFixturesIdByMatch(int matchId);
        bool MatchHasResult(int matchId);
        bool MatchHasCurrentReferee(int matchId, string userName);
    }
}