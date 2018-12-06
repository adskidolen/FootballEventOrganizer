namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
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

        public void CreateLeague(string name, string description, DateTime startDate, DateTime endDate, string townName)
        {
            var town = this.townsService.GetByName(townName);

            if (town == null)
            {
                town = this.townsService.CreateTown(townName);
            }

            if (this.ExistsByName(name))
            {
                // TODO: Error for existing league
            }

            var league = new League
            {
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                Town = town
            };

            this.dbContext.Leagues.Add(league);
            this.dbContext.SaveChanges();
        }

        public bool ExistsById(int id)
            => this.dbContext.Leagues.Any(l => l.Id == id);

        public bool ExistsByName(string name)
            => this.dbContext.Leagues.Any(l => l.Name == name);

        public League GetById(int id)
            => this.dbContext.Leagues.SingleOrDefault(l => l.Id == id);

        public League GetByName(string name)
            => this.dbContext.Leagues.SingleOrDefault(l => l.Name == name);

        public IQueryable<TModel> All<TModel>()
            => this.dbContext.Leagues.AsQueryable().ProjectTo<TModel>();
    }
}