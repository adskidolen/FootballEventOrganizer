namespace Footeo.Services.Contracts
{
    public interface IMatchesService
    {
        void CreateMatch(int homeTeamId, int awayTeamId, int refereeId, int fieldId, int fixtureId);
    }
}