using Microsoft.Extensions.Hosting;

namespace ActivitiesBot;

public class ActivitiesBot(DiscordSocketClient client, InteractionService commands, IConfiguration config, 
    ILogger<ActivitiesBot> logger, InteractionHandler interactionHandler) : IHostedService
{
    public async Task StartAsync(CancellationToken token)
    {
        client.Ready += ClientReady;
        client.JoinedGuild += JoinedGuild;
        client.LeftGuild += LeftGuild;

        client.Log += LogAsync;
        commands.Log += LogAsync;

        await interactionHandler.InitializeAsync();

        await client.LoginAsync(TokenType.Bot, config["BotToken"]);
        await client.StartAsync();
    }

    public async Task StopAsync(CancellationToken token)
    {
        await client.StopAsync();
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

        logger.LogInformation("Registering commands globally");
        await commands.RegisterCommandsGloballyAsync();

        await client.SetGameAsync($"Working on {client.Guilds.Count} servers");
    }

    public Task LogAsync(LogMessage msg)
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
        return Task.CompletedTask;
    }
}
