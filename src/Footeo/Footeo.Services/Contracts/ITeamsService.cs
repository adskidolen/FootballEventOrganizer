namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Collections.Generic;

    public interface ITeamsService
    {
        void CreateTeam(string name, string initials, string townName);
        bool Exists(int id);
        bool Exists(string name);
        Team GetById(int id);
        Team GetByName(string name);
        IEnumerable<Team> All();
    }
}