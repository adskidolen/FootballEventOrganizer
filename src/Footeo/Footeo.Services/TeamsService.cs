namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TeamsService : ITeamsService
    {
        private const string TeamAlreadyExistsErrorMessage = "Team already exists!";

        private readonly FooteoDbContext dbContext;

        public TeamsService(FooteoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Team> All()
            => this.dbContext.Teams.ToList();

        public void CreateTeam(string name, string initials, string logo, int townId)
        {
            if (this.ExistsByName(name))
            {
                throw new InvalidOperationException(TeamAlreadyExistsErrorMessage);
            }

            var team = new Team
            {
                Name = name,
                Initials = initials,
                LogoUrl = logo,
                TownId = townId
            };

            this.dbContext.Teams.Add(team);
            this.dbContext.SaveChanges();
        }

        public bool ExistsById(int id)
            => this.dbContext.Teams.Any(t => t.Id == id);

        public bool ExistsByName(string name)
            => this.dbContext.Teams.Any(t => t.Name == name);

        public Team GetById(int id)
            => this.dbContext.Teams.FirstOrDefault(t => t.Id == id);

        public Team GetByName(string name)
            => this.dbContext.Teams.FirstOrDefault(t => t.Name == name);
    }
}