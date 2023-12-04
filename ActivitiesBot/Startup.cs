global using Discord;
global using Discord.Interactions;
global using Discord.WebSocket;
global using System.Globalization;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Localization;
global using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using ActivitiesBot;
using ActivitiesBot.Database;
using Serilog;
using Microsoft.Extensions.Hosting;


var builder = new HostBuilder();

var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File($"logs/log-{DateTime.Now:yy.MM.dd_HH.mm}.log")
    .CreateLogger();

var activities = new AllActivities
{
    Activities = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("activities.json"))
};

if (activities.Activities is null)
    throw new FileNotFoundException("Missing activities.json");


builder.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: true);
    config.AddEnvironmentVariables("ACTIVITIES_");
});

builder.ConfigureServices((host, services) =>
{
    services.AddLogging(options => options.AddSerilog(loggerConfig, true));


    services.AddSingleton(activities);

    //Modify this line if using different DB engine
    services.AddDbContext<ActivitiesDBContext>(options => options.UseSqlServer(host.Configuration.GetConnectionString("ActivitiesBot")));

    services.AddSingleton(new DiscordSocketClient(
        new DiscordSocketConfig
        {
            FormatUsersInBidirectionalUnicode = false,
            LogGatewayIntentWarnings = false
        }));
    services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
    services.AddSingleton<InteractionHandler>();

    services.AddTransient<LangProvider>();
    services.AddLocalization(options => options.ResourcesPath = "Resources");

    services.AddHostedService<ActivitiesBot.ActivitiesBot>();
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var prov = scope.ServiceProvider.GetRequiredService<ActivitiesDBContext>();
    await prov.Database.EnsureCreatedAsync();
}

await app.RunAsync();

