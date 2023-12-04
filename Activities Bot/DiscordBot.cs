namespace Activities_Bot;

public class Activities_Bot(DiscordSocketClient client, InteractionService commands, IConfiguration config, ILogger<Activities_Bot> logger, InteractionHandler interactionHandler)
{
    public async Task StartAsync()
    {
        client.Ready += ClientReady;
        client.JoinedGuild += JoinedGuild;
        client.LeftGuild += LeftGuild;

        client.Log += LogAsync;
        commands.Log += LogAsync;

        await interactionHandler.InitializeAsync();

        await client.LoginAsync(TokenType.Bot, config["BotToken"]);
        await client.StartAsync();


        await Task.Delay(-1);
    }

    private async Task LeftGuild(SocketGuild arg)
    {
        await client.SetGameAsync($"Working on {client.Guilds.Count} servers");
    }

    private async Task JoinedGuild(SocketGuild arg)
    {

        await client.SetGameAsync($"Working on {client.Guilds.Count} servers");
    }

    private async Task ClientReady()
    {
        logger.LogInformation($"Logged as {client.CurrentUser}");

        logger.LogWarning("Registering commands globally");
        await commands.RegisterCommandsGloballyAsync();

        await client.SetGameAsync($"Working on {client.Guilds.Count} servers");
    }

    public async Task LogAsync(LogMessage msg)
    {
        var severity = msg.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };

        logger.Log(severity, msg.Exception, msg.Message);

        await Task.CompletedTask;
    }
}
