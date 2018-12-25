namespace Footeo.Services
{
    using AutoMapper.QueryableExtensions;

    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MatchesService : IMatchesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly IFixturesService fixturesService;
        private readonly ITeamsService teamsService;

        public MatchesService(FooteoDbContext dbContext,
            IFixturesService fixturesService, ITeamsService teamsService)
        {
            this.dbContext = dbContext;
            this.fixturesService = fixturesService;
            this.teamsService = teamsService;
        }

        public void CreateMatch(int homeTeamId, int awayTeamId, int fieldId, int fixtureId)
        {
            var fixture = this.fixturesService.GetFixtureById<Fixture>(fixtureId);

            var match = new Match
            {
                Date = fixture.Date,
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                FieldId = fieldId,
                FixtureId = fixtureId
            };

            var homeTeam = this.teamsService.GetTeamById<Team>(homeTeamId);
            homeTeam.HomeMatches.Add(match);

            var awayTeam = this.teamsService.GetTeamById<Team>(awayTeamId);
            awayTeam.AwayMatches.Add(match);

            this.dbContext.Matches.Add(match);
            this.dbContext.SaveChanges();
        }

        public int GetFixturesIdByMatch(int matchId)
            => this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId).FixtureId;

        public TModel GetMatchById<TModel>(int id)
            => this.By<TModel>(m => m.Id == id).SingleOrDefault();

        public IQueryable<TModel> MatchesByFixture<TModel>(int fixtureId)
            => this.dbContext.Matches.Where(f => f.FixtureId == fixtureId).AsQueryable().ProjectTo<TModel>();

        public bool MatchExistsById(int id)
            => this.dbContext.Matches.Any(m => m.Id == id);

        public bool MatchHasCurrentReferee(int matchId, string userName)
        {
            var match = this.dbContext.Matches.FirstOrDefault(m => m.Id == matchId);
            var refereeId = this.dbContext.Users.FirstOrDefault(u => u.UserName == userName).RefereeId;

            if (match.RefereeId != refereeId)
            {
                return false;
            }

            return true;
        }

        public bool MatchHasReferee(int matchId)
            => this.dbContext.Matches.Where(m => m.Id == matchId).Any(r => r.RefereeId != null);

        public bool MatchHasResult(int matchId)
            => this.dbContext.Matches.Where(m => m.Id == matchId).Any(r => r.Result != null);

        private IEnumerable<TModel> By<TModel>(Func<Match, bool> predicate)
            => this.dbContext.Matches.Where(predicate).AsQueryable().ProjectTo<TModel>();
    }
}