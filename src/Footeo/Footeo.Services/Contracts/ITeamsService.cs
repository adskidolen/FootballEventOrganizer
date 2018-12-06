namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Collections.Generic;

    public interface ITeamsService
    {
        void CreateTeam(string name, string initials, string townName, string userName);
        bool ExistsById(int id);
        bool ExistsByName(string name);
        Team GetById(int id);
        Team GetByName(string name);
        IEnumerable<Team> All();
        IEnumerable<Player> Players(int teamId);
        void JoinTeam(int teamId, string userName);
    }
}