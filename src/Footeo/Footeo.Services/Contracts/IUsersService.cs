namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    public interface IUsersService
    {
        void CreatePlayer(FooteoUser user, Player player);
        void CreateReferee(FooteoUser user, Referee referee);
        bool HasATeam(string userName);
        void SetNickname(string userName, string nickname);
    }
}