namespace Footeo.Services
{
    using Footeo.Common;
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using Microsoft.AspNetCore.Identity;

    using System.Collections.Generic;
    using System.Linq;

    public class TeamsService : ITeamsService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ITownsService townsService;
        private readonly UserManager<FooteoUser> userManager;

        public TeamsService(FooteoDbContext dbContext, ITownsService townsService, UserManager<FooteoUser> userManager)
        {
            this.dbContext = dbContext;
            this.townsService = townsService;
            this.userManager = userManager;
        }

        public IEnumerable<Team> All()
            => this.dbContext.Teams.ToList();

        public void CreateTeam(string name, string initials, string townName, string userName)
        {
            var town = this.townsService.GetByName(townName);

            if (town == null)
            {
                town = this.townsService.CreateTown(townName);
            }

            var team = new Team
            {
                Name = name,
                Initials = initials,
                Town = town
            };

            var player = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault().Player;
            player.Team = team;
            player.IsCaptain = true;

            team.Players.Add(player);

            this.dbContext.Teams.Add(team);
            this.dbContext.SaveChanges();

            //var user = this.userManager.Users.FirstOrDefault(u => u.UserName == userName);
            //this.userManager.RemoveFromRoleAsync(user, GlobalConstants.PlayerRoleName);
            //this.userManager.AddToRoleAsync(user, GlobalConstants.PlayerInTeamRoleName);
        }

        public bool ExistsById(int id)
            => this.dbContext.Teams.Any(t => t.Id == id);

        public bool ExistsByName(string name)
            => this.dbContext.Teams.Any(t => t.Name == name);

        public Team GetById(int id)
            => this.dbContext.Teams.SingleOrDefault(t => t.Id == id);

        public Team GetByName(string name)
            => this.dbContext.Teams.SingleOrDefault(t => t.Name == name);

        public void JoinTeam(int teamId, string userName)
        {
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            var team = this.GetById(teamId);

            team.Players.Add(user.Player);
            this.dbContext.SaveChanges();

            //this.userManager.RemoveFromRoleAsync(user, GlobalConstants.PlayerRoleName);
            //this.userManager.AddToRoleAsync(user, GlobalConstants.PlayerInTeamRoleName);
        }

        public IEnumerable<Player> Players(int teamId)
        {
            var team = this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId);

            return team.Players;
        }
    }
}