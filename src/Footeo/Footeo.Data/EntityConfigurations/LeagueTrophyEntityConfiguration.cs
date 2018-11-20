namespace Footeo.Data.EntityConfigurations
{
    using Footeo.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LeagueTrophyEntityConfiguration : IEntityTypeConfiguration<LeagueTrophy>
    {
        public void Configure(EntityTypeBuilder<LeagueTrophy> builder)
        {
            builder.HasKey(pk => new
            {
                pk.LeagueId,
                pk.TrophyId
            });
        }
    }
}