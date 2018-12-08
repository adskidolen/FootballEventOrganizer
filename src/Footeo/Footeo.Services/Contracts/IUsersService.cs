namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System.Linq;

    public interface IUsersService
    {
        void CreatePlayer(FooteoUser user, Player player);
        void CreateReferee(FooteoUser user, Referee referee);
        bool PlayerHasATeam(string userName);
        void SetPlayersNickname(string userName, string nickname);
        IQueryable<TModel> PlayersByTeam<TModel>(int teamId);
        void JoinTeam(int teamId, string userName);
    }
}