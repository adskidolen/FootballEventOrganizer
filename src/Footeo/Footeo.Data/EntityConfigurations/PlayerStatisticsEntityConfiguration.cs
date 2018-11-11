namespace Footeo.Data.EntityConfigurations
{
    using Footeo.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerStatisticsEntityConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder.HasKey(pk => new
            {
                pk.PlayerId,
                pk.MatchId
            });
        }
    }
}