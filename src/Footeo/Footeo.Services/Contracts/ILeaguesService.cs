namespace Footeo.Services.Contracts
{
    using System;
    using System.Linq;

    public interface ILeaguesService
    {
        void CreateLeague(string name, string description, DateTime startDate, DateTime endDate, string town);
        bool LeagueExistsById(int id);
        bool LeagueExistsByName(string name);
        TModel GetLeagueById<TModel>(int id);
        TModel GetLeagueByName<TModel>(string name);
        IQueryable<TModel> AllLeagues<TModel>();
    }
}