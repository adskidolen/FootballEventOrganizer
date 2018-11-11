namespace Footeo.Data.EntityConfigurations
{
    using Footeo.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MatchEntityConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasOne(ht => ht.HomeTeam)
                   .WithMany(hm => hm.HomeMatches)
                   .HasForeignKey(fk => fk.HomeTeamId);

            builder.HasOne(at => at.AwayTeam)
                   .WithMany(am => am.AwayMatches)
                   .HasForeignKey(fk => fk.AwayTeamId);
        }
    }
}