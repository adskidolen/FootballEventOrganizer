namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System.Linq;

    public class UsersService : IUsersService
    {
        private readonly FooteoDbContext dbContext;

        public UsersService(FooteoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreatePlayer(FooteoUser user, Player player)
        {
            user.Player = player;
            this.dbContext.SaveChanges();
        }

        public void CreateReferee(FooteoUser user, Referee referee)
        {
            user.Referee = referee;
            this.dbContext.SaveChanges();
        }

        public bool HasATeam(string userName)
            => this.dbContext.Users.Where(u => u.UserName == userName).Any(p => p.Player.TeamId != null);
    }
}