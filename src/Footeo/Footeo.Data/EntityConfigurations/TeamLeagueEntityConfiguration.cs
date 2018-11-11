namespace Footeo.Data.EntityConfigurations
{
    using Footeo.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TeamLeagueEntityConfiguration : IEntityTypeConfiguration<TeamLeague>
    {
        public void Configure(EntityTypeBuilder<TeamLeague> builder)
        {
            builder.HasKey(pk => new
            {
                pk.TeamId,
                pk.LeagueId
            });
        }
    }
}