namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    public class MatchesService : IMatchesService
    {
        private readonly FooteoDbContext dbContext;
        private readonly IFixturesService fixturesService;

        public MatchesService(FooteoDbContext dbContext, IFixturesService fixturesService)
        {
            this.dbContext = dbContext;
            this.fixturesService = fixturesService;
        }

        public void CreateMatch(int homeTeamId, int awayTeamId, int refereeId, int fieldId, int fixtureId)
        {
            var match = new Match
            {
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                RefereeId = refereeId,
                FieldId = fieldId,
                FixtureId = fixtureId
            };

            this.dbContext.Matches.Add(match);
            this.dbContext.SaveChanges();
        }
    }
}