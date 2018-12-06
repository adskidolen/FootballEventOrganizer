namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System;
    using System.Linq;

    public interface ILeaguesService
    {
        void CreateLeague(string name, string description, DateTime startDate, DateTime endDate, string town);
        bool ExistsById(int id);
        bool ExistsByName(string name);
        League GetById(int id);
        League GetByName(string name);
        IQueryable<TModel> All<TModel>();
    }
}