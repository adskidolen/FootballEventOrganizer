namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
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

        public void CreateLeague(string name, string description, DateTime startDate, DateTime endDate, string townName)
        {
            var town = this.townsService.GetTownByName<Town>(townName);

            if (town == null)
            {
                town = this.townsService.CreateTown(townName);
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

        public bool LeagueExistsById(int id)
            => this.dbContext.Leagues.Any(l => l.Id == id);

        public bool LeagueExistsByName(string name)
            => this.dbContext.Leagues.Any(l => l.Name == name);

        public TModel GetLeagueById<TModel>(int id)
            => this.By<TModel>(l => l.Id == id).SingleOrDefault();

        public TModel GetLeagueByName<TModel>(string name)
            => this.By<TModel>(l => l.Name == name).SingleOrDefault();

        public IQueryable<TModel> AllLeagues<TModel>()
            => this.dbContext.Leagues.AsQueryable().ProjectTo<TModel>();

        private IEnumerable<TModel> By<TModel>(Func<League, bool> predicate)
           => this.dbContext.Leagues.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}