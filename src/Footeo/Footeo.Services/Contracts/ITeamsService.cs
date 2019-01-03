namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Collections.Generic;
    using System.Linq;

    public interface ITeamsService
    {
        void CreateTeam(string name, string initials, string userName);
        bool TeamExistsById(int id);
        bool TeamExistsByName(string name);
        TModel GetTeamById<TModel>(int id);
        IQueryable<TModel> AllTeams<TModel>();
        int PlayersCount(int teamId);
        bool IsTeamInLeague(int teamId);
        Team GetUsersTeam(string userName);
        IQueryable<TModel> AllTrophiesByTeamId<TModel>(int teamId);
        IEnumerable<Match> AllHomeMatchesByTeamId(int teamId);
        IEnumerable<Match> AllAwayMatchesByTeamId(int teamId);
    }
}