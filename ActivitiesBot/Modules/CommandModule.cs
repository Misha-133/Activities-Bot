using ActivitiesBot.Database;

namespace ActivitiesBot.Modules;


public class CommandModule : ModuleBase
{
    private string? GuildCulture;

    public override Task BeforeExecuteAsync(ICommandInfo command)
    {
        if(Context.Interaction is not SocketAutocompleteInteraction && Context.Channel is not SocketDMChannel)
            GuildCulture = _db.guildLocales.FirstOrDefault(x => x.GuildId == Context.Guild.Id)?.Culture ?? "en";

        return base.BeforeExecuteAsync(command);
    }

    [SlashCommand("help", "Show bot help")]
    public async Task HelpCommand()
    {
        await RespondAsync(embed: new EmbedBuilder()
                                  .WithTitle(_langProvider.GetString("HelpTitle", GuildCulture))
                                  .WithDescription(_langProvider.GetString("HelpDescription", GuildCulture))
                                  .WithColor(0xff00)
                                  .Build());
    }

    [DefaultMemberPermissions(GuildPermission.ManageGuild)]
    [SlashCommand("set-locale", "Set bot's language")]
    public async Task SetLocaleCommand([Choice("English", "en"), Choice("Русский", "ru"), Summary("language", "Choose language")]string locale)
    {
        if (Context.Channel is IDMChannel)
        {
            await RespondAsync(embed: new EmbedBuilder().WithTitle(_langProvider.GetString("OnlyInGuilds")).WithColor(0xff0000).Build());
            return;
        }

        (_db.guildLocales.FirstOrDefault(x => x.GuildId == Context.Guild.Id) ?? _db.guildLocales.Add(new GuildLocale { GuildId = Context.Guild.Id }).Entity).Culture = locale;
        _db.SaveChanges();

        await RespondAsync(_langProvider.GetString("LocaleSet", locale), ephemeral: true);
    }

    [SlashCommand("invite", "Invite this bot to your server")]
    public async Task InviteBotCommand()
    {
        await RespondAsync($"**[ActivitiesBot]({_config.GetValue<string>("InviteUrl")})**", ephemeral: true);
    }

    [SlashCommand("activity", "Start Discord Activity")]
    public async Task StartActivityCommand([Summary("activity", "Choose activity"), Autocomplete()] string activity)
    {
        if (Context.Channel is IDMChannel)
        {
            await RespondAsync(embed: new EmbedBuilder().WithTitle(_langProvider.GetString("OnlyInGuilds", GuildCulture)).WithColor(0xff0000).Build());
            return;
        }

        if ((Context.User as SocketGuildUser).VoiceChannel is null)
        {
            await RespondAsync(embed: new EmbedBuilder().WithTitle(_langProvider.GetString("MustBeInVoice", GuildCulture)).WithColor(0xff0000).Build());
            return;
        }

        var link = await (Context.User as SocketGuildUser).VoiceChannel.CreateInviteToApplicationAsync(ulong.Parse(activity), 3600);

        var emb = new EmbedBuilder()
            .WithTitle(_langProvider.GetString("YourActivity", GuildCulture))
            .WithDescription($"`{_activities.Activities[activity]}`")
            .WithColor(0xff00)
            .Build();
        await RespondAsync(embed: emb, 
            components: new ComponentBuilder()
            .WithButton(_langProvider.GetString("ConnectButtonText", GuildCulture), url: link.Url, style: ButtonStyle.Link)
            .Build());
    }

    [AutocompleteCommand("activity", "activity")]
    public async Task ActivityAutocomplete()
    {
        IEnumerable<AutocompleteResult> results;

        var interaction = Context.Interaction as SocketAutocompleteInteraction;

        results = _activities.Activities.Where(x => x.Value.ToLower().Contains(interaction.Data.Current.Value.ToString().ToLower() ?? "")).Select(x => new AutocompleteResult { Name = x.Value, Value = x.Key });
    
        await interaction.RespondAsync(results);
    }
}
