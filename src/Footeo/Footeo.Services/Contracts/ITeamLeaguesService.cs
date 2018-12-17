namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Linq;

    public interface ITeamLeaguesService
    {
        void JoinLeague(string userName, int leagueId);
        IQueryable<TModel> LeagueTable<TModel>(int leagueId);
        bool IsTeamInLeague(int leagueId, string userName);
        int TeamsCount(int leagueId);
        TeamLeague GetTeamLeague(int teamId);
    }
}