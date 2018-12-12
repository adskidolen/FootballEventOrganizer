namespace Footeo.Data
{
    using Footeo.Data.EntityConfigurations;
    using Footeo.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FooteoDbContext : IdentityDbContext<FooteoUser>
    {
        public DbSet<FooteoUser> FooteoUsers { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayersStatistics { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Trophy> Trophies { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<TeamLeague> TeamsLeagues { get; set; }
        public DbSet<LeagueTrophy> LeaguesTrophies { get; set; }

        public FooteoDbContext() { }

        public FooteoDbContext(DbContextOptions<FooteoDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new MatchEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerStatisticsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamLeagueEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LeagueTrophyEntityConfiguration());
        }
    }
}