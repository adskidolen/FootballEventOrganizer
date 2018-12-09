namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Common;
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using Microsoft.AspNetCore.Identity;

    using System.Linq;

    public class UsersService : IUsersService
    {
        private readonly FooteoDbContext dbContext;
        private readonly UserManager<FooteoUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITeamsService teamsService;

        public UsersService(FooteoDbContext dbContext,
            UserManager<FooteoUser> userManager, RoleManager<IdentityRole> roleManager,
            ITeamsService teamsService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.teamsService = teamsService;
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

        public bool PlayerHasATeam(string userName)
            => this.dbContext.Users
                             .Where(u => u.UserName == userName)
                             .Any(p => p.Player.TeamId != null);

        public void SetPlayersNickname(string userName, string nickname)
        {
            var player = this.dbContext.Users
                                       .Where(u => u.UserName == userName)
                                       .Select(p => p.Player)
                                       .FirstOrDefault();

            player.Nickname = nickname;

            this.dbContext.SaveChanges();
        }

        public void JoinTeam(int teamId, string userName)
        {
            var team = this.teamsService.GetTeamById<Team>(teamId);
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == userName);

            var removeResult = this.userManager.RemoveFromRoleAsync(user, GlobalConstants.PlayerRoleName).GetAwaiter().GetResult();
            var addResult = this.userManager.AddToRoleAsync(user, GlobalConstants.PlayerInTeamRoleName).GetAwaiter().GetResult();

            if (removeResult.Succeeded && addResult.Succeeded)
            {
                team.Players.Add(user.Player);
                this.dbContext.SaveChanges();
            }
        }

        public IQueryable<TModel> PlayersByTeam<TModel>(int teamId)
            => this.dbContext.Teams
                             .FirstOrDefault(t => t.Id == teamId)
                             .Players
                             .AsQueryable()
                             .ProjectTo<TModel>();
    }
}