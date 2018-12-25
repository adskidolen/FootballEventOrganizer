namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Common;
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Models.Enums;
    using Footeo.Services.Contracts;

    using Microsoft.AspNetCore.Identity;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayersService : IPlayersService
    {
        private readonly FooteoDbContext dbContext;
        private readonly UserManager<FooteoUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITeamsService teamsService;

        public PlayersService(FooteoDbContext dbContext,
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

        public bool PlayerHasATeam(string userName)
            => this.dbContext.Users.Where(u => u.UserName == userName).Any(p => p.Player.TeamId != null);

        public void SetPlayersNickname(string userName, string nickname)
        {
            var user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var player = user.Player;

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

        public TModel GetPlayerByName<TModel>(string playerName)
            => this.By<TModel>(p => p.FullName == playerName).SingleOrDefault();

        private IEnumerable<TModel> By<TModel>(Func<Player, bool> predicate)
       => this.dbContext.Players.Where(predicate).AsQueryable().ProjectTo<TModel>();

        public void SetSquadNumber(string userName, int squadNumber)
        {
            var user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var player = user.Player;

            player.SquadNumber = squadNumber;

            this.dbContext.SaveChanges();
        }

        public void SetPosition(string userName, PlayerPosition position)
        {
            var user = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var player = user.Player;

            player.Position = position;

            this.dbContext.SaveChanges();
        }

        public bool IsSquadNumberTaken(int squadNumber)
            => this.dbContext.Players.Any(sn => sn.SquadNumber == squadNumber);
    }
}