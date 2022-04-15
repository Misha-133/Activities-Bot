using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;

using Microsoft.Extensions.DependencyInjection;

namespace Activities_Bot.Database;

public class ActivitiesDBContext : DbContext
{
    public DbSet<GuildLocale> guildLocales { get; set; }

    public ActivitiesDBContext(DbContextOptions<ActivitiesDBContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
{
        builder.Entity<GuildLocale>()
            .HasIndex(g => g.GuildId)
            .IsUnique();
    }
}
