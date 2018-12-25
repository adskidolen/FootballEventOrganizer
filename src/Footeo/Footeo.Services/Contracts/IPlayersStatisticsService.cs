namespace Footeo.Services.Contracts
{
    public interface IPlayersStatisticsService
    {
        void CreatePlayerStatistics(int matchId, int playerId, int goalsScored, int assists);
    }
}