namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System.Collections.Generic;
    using System.Linq;

    public class LeagueService : ILeaguesService
    {
        private readonly FooteoDbContext dbContext;

        private readonly ITownsService townsService;

        public LeagueService(FooteoDbContext dbContext, ITownsService townsService)
        {
            this.dbContext = dbContext;
            this.townsService = townsService;
        }

        public void CreateLeague(string name, string description, string townName)
        {
            var town = this.townsService.GetByName(townName);

            if (town == null)
            {
                town = this.townsService.Create(townName);
            }

            var league = new League
            {
                Name = name,
                Description = description,
                Town = town
            };

            this.dbContext.Leagues.Add(league);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<League> All()
            => this.dbContext.Leagues.ToList();

        public bool ExistsById(int id)
            => this.dbContext.Leagues.Any(l => l.Id == id);

        public bool ExistsByName(string name)
            => this.dbContext.Leagues.Any(l => l.Name == name);

        public League GetById(int id)
            => this.dbContext.Leagues.SingleOrDefault(l => l.Id == id);

        public League GetByName(string name)
            => this.dbContext.Leagues.SingleOrDefault(l => l.Name == name);

        public Town GetTown(string name) => this.townsService.GetByName(name);
    }
}