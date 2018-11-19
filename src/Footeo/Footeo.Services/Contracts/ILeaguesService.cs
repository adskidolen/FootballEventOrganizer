namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Collections.Generic;

    public interface ILeaguesService
    {
        void CreateLeague(string name, string description, string town);
        bool ExistsById(int id);
        bool ExistsByName(string name);
        League GetById(int id);
        League GetByName(string name);
        IEnumerable<League> All();
        Town GetTown(string name);
    }
}