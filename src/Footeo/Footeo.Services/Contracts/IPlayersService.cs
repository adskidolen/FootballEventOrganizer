namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Linq;

    public interface IPlayersService
    {
        void CreatePlayer(FooteoUser user, Player player);
        bool PlayerHasATeam(string userName);
        void SetPlayersNickname(string userName, string nickname);
        IQueryable<TModel> PlayersByTeam<TModel>(int teamId);
        void JoinTeam(int teamId, string userName);
    }
}