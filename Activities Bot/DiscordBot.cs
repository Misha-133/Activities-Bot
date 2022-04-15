using System.Text;

using Newtonsoft.Json;

namespace Activities_Bot;

public class Activities_Bot
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _commands;
    private readonly IConfiguration _config;
    private readonly ILogger<Activities_Bot> _logger;
    private readonly InteractionHandler _interactionHandler;
    private AllActivities _allActivities;

    public Activities_Bot(DiscordSocketClient client, InteractionService commands, IConfiguration config, ILogger<Activities_Bot> logger, InteractionHandler interactionHandler, AllActivities allActivities)
    {
        _client = client;
        _commands = commands;
        _config = config;
        _logger = logger;
        _interactionHandler = interactionHandler;
        _allActivities = allActivities;
    }

    public async Task StartAsync()
    {
        _client.Ready += ClientReady;

        _client.Log += LogAsync;
        _commands.Log += LogAsync;

        await _interactionHandler.InitializeAsync();

        await _client.LoginAsync(TokenType.Bot, _config["BotToken"]);
        await _client.StartAsync();


        await Task.Delay(-1);
    }

    private async Task ClientReady()
    {
        _logger.LogInformation($"Logged as {_client.CurrentUser}");

        if (IsDebug())
        {
            _logger.LogWarning("Debug environment; Registering commands to dev server");
            await _commands.RegisterCommandsToGuildAsync(ulong.Parse(_config["Development:ServerId"]));
        }
        else
        {
            _logger.LogWarning("Production environment; Registering commands globally");
            await _commands.RegisterCommandsGloballyAsync();
        }

        await _client.Rest.DeleteAllGlobalCommandsAsync();

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

        _logger.Log(severity, msg.Exception, msg.Message);

        await Task.CompletedTask;
    }

    static bool IsDebug()
    {
        #if DEBUG
            return true;
        #else
            return false;
        #endif
    }
}
