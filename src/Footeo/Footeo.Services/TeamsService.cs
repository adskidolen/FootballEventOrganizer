namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System.Collections.Generic;
    using System.Linq;

    public class TeamsService : ITeamsService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ITownsService townsService;

        public TeamsService(FooteoDbContext dbContext, ITownsService townsService)
        {
            this.dbContext = dbContext;
            this.townsService = townsService;
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

            team.Players.Add(player);

            this.dbContext.Teams.Add(team);
            this.dbContext.SaveChanges();
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
            var team = this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId);
            var player = this.dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault().Player;

            team.Players.Add(player);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Player> Players(int teamId)
        {
            var team = this.dbContext.Teams.FirstOrDefault(t => t.Id == teamId);

            return team.Players;
        }
    }
}