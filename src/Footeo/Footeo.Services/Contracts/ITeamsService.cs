﻿namespace Footeo.Services.Contracts
{
    using System.Linq;

    public interface ITeamsService
    {
        void CreateTeam(string name, string initials, string townName, string userName);
        bool TeamExistsById(int id);
        bool TeamExistsByName(string name);
        TModel GetTeamById<TModel>(int id);
        TModel GetTeamByName<TModel>(string name);
        IQueryable<TModel> AllTeams<TModel>();
    }
}