namespace Footeo.Services
{
    using Footeo.Data;
    using Footeo.Models;
    using Footeo.Services.Contracts;

    public class PlayersStatisticsService : IPlayersStatisticsService
    {
        private readonly FooteoDbContext dbContext;

        public PlayersStatisticsService(FooteoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreatePlayerStatistics(int matchId, int playerId, int goalsScored, int assists)
        {
            var playerStatistic = new PlayerStatistic
            {
                MatchId = matchId,
                PlayerId = playerId,
                GoalsScored = goalsScored,
                Assists = assists
            };

            //  this.dbContext.PlayersStatistics.Add(playerStatistic);
            //  this.dbContext.SaveChanges();
        }
    }
}
