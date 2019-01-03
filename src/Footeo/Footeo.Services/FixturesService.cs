namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using System;
    using System.Linq;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System.Collections.Generic;

    public class FixturesService : IFixturesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly ILeaguesService leaguesService;

        public FixturesService(FooteoDbContext dbContext, ILeaguesService leaguesService)
        {
            this.dbContext = dbContext;
            this.leaguesService = leaguesService;
        }

        public void CreateFixture(string name, DateTime date, int leagueId)
        {
            var league = this.leaguesService.GetLeagueById<League>(leagueId);

            var fixture = new Fixture
            {
                Name = name,
                Date = date,
                League = league
            };

            this.dbContext.Fixtures.Add(fixture);
            this.dbContext.SaveChanges();
        }

        public IQueryable<TModel> AllFixtures<TModel>(int leagueId)
            => this.dbContext.Fixtures.Where(x => x.LeagueId == leagueId).AsQueryable().ProjectTo<TModel>();

        public League GetLeagueForFixture(int fixtureId)
            => this.dbContext.Fixtures.FirstOrDefault(f => f.Id == fixtureId).League;

        public bool FixtureExistsById(int id)
            => this.dbContext.Fixtures.Any(f => f.Id == id);

        public TModel GetFixtureById<TModel>(int id)
            => this.By<TModel>(f => f.Id == id).SingleOrDefault();

        private IEnumerable<TModel> By<TModel>(Func<Fixture, bool> predicate)
            => this.dbContext.Fixtures.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}