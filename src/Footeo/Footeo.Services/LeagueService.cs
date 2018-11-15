namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LeagueService : ILeaguesService
    {
        private const string AlreadyExistsLeagueMessage = "League already exists!";

        private readonly FooteoDbContext dbContext;

        public LeagueService(FooteoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<League> All() => this.dbContext.Leagues.ToList();

        public void CreateLeague(string name, string description, string news, int townId)
        {
            if (this.ExistsByName(name))
            {
                throw new InvalidOperationException(AlreadyExistsLeagueMessage);
            }

            var league = new League
            {
                Name = name,
                Description = description,
                News = news,
                TownId = townId
            };

            this.dbContext.Leagues.Add(league);
            this.dbContext.SaveChanges();
        }

        public bool ExistsById(int id)
            => this.dbContext.Leagues.Any(l => l.Id == id);

        public bool ExistsByName(string name)
            => this.dbContext.Leagues.Any(l => l.Name == name);

        public League GetById(int id)
            => this.dbContext.Leagues.FirstOrDefault(l => l.Id == id);

        public League GetByName(string name)
            => this.dbContext.Leagues.FirstOrDefault(l => l.Name == name);
    }
}