namespace Footeo.Services.Contracts
{
    using Footeo.Models;

    using System;
    using System.Linq;

    public interface IFixturesService
    {
        void CreateFixture(DateTime date, int leagueId);
        IQueryable<TModel> AllFixtures<TModel>(int leagueId);
        League GetLeagueForFixture(int fixtureId);
        bool FixtureExistsById(int id);
    }
}