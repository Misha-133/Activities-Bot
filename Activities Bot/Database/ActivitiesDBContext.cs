namespace ActivitiesBot.Database;

public class ActivitiesDBContext(DbContextOptions<ActivitiesDBContext> options) : DbContext(options)
{
    public DbSet<GuildLocale> guildLocales { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
{
        builder.Entity<GuildLocale>()
            .HasIndex(g => g.GuildId)
            .IsUnique();
    }
}
