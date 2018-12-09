using System.Linq;

namespace Footeo.Services.Contracts
{
    public interface ITeamLeaguesService
    {
        void JoinLeague(string userName, int leagueId);
        IQueryable<TModel> LeagueTable<TModel>(int leagueId);
    }
}