namespace Footeo.Services.Contracts
{
    using Footeo.Models;
    using Footeo.Models.Enums;

    using System.Linq;

    public interface IPlayersService
    {
        void CreatePlayer(FooteoUser user, Player player);
        bool PlayerHasATeam(string userName);
        void SetPlayersNickname(string userName, string nickname);
        IQueryable<TModel> PlayersByTeam<TModel>(int teamId);
        void JoinTeam(int teamId, string userName);
        TModel GetPlayerById<TModel>(int id);
        void SetSquadNumber(string userName, int squadNumber);
        void SetPosition(string userName, PlayerPosition position);
        bool IsSquadNumberTaken(int squadNumber, int teamId);
        bool PlayerExistsById(int id);
    }
}