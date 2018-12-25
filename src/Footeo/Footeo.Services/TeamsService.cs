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

    public class TeamsService : ITeamsService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ITownsService townsService;
        private readonly ILeaguesService leaguesService;
        private readonly UserManager<FooteoUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TeamsService(FooteoDbContext dbContext,
            ITownsService townsService, ILeaguesService leaguesService,
            UserManager<FooteoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.townsService = townsService;
            this.leaguesService = leaguesService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void CreateTeam(string name, string initials, string userName)
        {
            var user = this.userManager.Users.FirstOrDefault(u => u.UserName == userName);

            var team = new Team
            {
                Name = name,
                Initials = initials,
                Town = user.Town
            };

            var removeResult = this.userManager.RemoveFromRoleAsync(user, GlobalConstants.PlayerRoleName).GetAwaiter().GetResult();
            var addPlayerInTeamResult = this.userManager.AddToRoleAsync(user, GlobalConstants.PlayerInTeamRoleName).GetAwaiter().GetResult();
            var addCaptainResult = this.userManager.AddToRoleAsync(user, GlobalConstants.CaptainRoleName).GetAwaiter().GetResult();

            if (removeResult.Succeeded && addPlayerInTeamResult.Succeeded && addCaptainResult.Succeeded)
            {
                user.Player.Team = team;
                user.Player.IsCaptain = true;

                team.Players.Add(user.Player);

                this.dbContext.Teams.Add(team);
                this.dbContext.SaveChanges();
            }
        }

        public bool TeamExistsById(int id)
            => this.dbContext.Teams.Any(t => t.Id == id);

        public bool TeamExistsByName(string name)
            => this.dbContext.Teams.Any(t => t.Name == name);

        public TModel GetTeamById<TModel>(int id)
            => this.By<TModel>(t => t.Id == id).SingleOrDefault();

        public TModel GetTeamByName<TModel>(string name)
            => this.By<TModel>(t => t.Name == name).SingleOrDefault();

        public IQueryable<TModel> AllTeams<TModel>()
            => this.dbContext.Teams.AsQueryable().ProjectTo<TModel>();

        public int PlayersCount(int teamId)
            => this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId).Players.Count;

        public bool IsTeamInLeague(int teamId)
            => this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId).Leagues
                                   .Any(l => l.League != null && l.League.Status != LeagueStatus.Completed);

        public Team GetUsersTeam(string userName)
            => this.dbContext.Users.FirstOrDefault(u => u.UserName == userName).Player.Team;

        private IEnumerable<TModel> By<TModel>(Func<Team, bool> predicate)
        => this.dbContext.Teams.Where(predicate).AsQueryable().ProjectTo<TModel>();

        public IQueryable<TModel> AllTrophiesByTeamId<TModel>(int teamId)
            => this.dbContext.Trophies.Where(t => t.TeamId == teamId).AsQueryable().ProjectTo<TModel>();

        public IEnumerable<Match> AllHomeMatchesByTeamId(int teamId)
            => this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId).HomeMatches.Where(r => r.Result != null);

        public IEnumerable<Match> AllAwayMatchesByTeamId(int teamId)
            => this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId).AwayMatches.Where(r => r.Result != null);
    }
}