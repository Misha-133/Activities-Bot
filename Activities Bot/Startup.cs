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
using Activities_Bot;
using Serilog;

[assembly: RootNamespace("Activities_Bot")]


IConfiguration config = new ConfigurationBuilder()
 .AddJsonFile("appsettings.json", optional: true)
 .AddEnvironmentVariables()
 .Build();

var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File($"logs/log-{DateTime.Now:dd.MM.yy_HH.mm}.log")
    .CreateLogger();

var activities = new AllActivities();
activities.Activities = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("activities.json"));

if (activities.Activities is null)
{
    throw new FileNotFoundException("Missing activities.json");
}

var services = new ServiceCollection();

services.AddSingleton(config);
services.AddSingleton(activities);

//Modify this line if using different DB engine
services.AddDbContext<Activities_Bot.Database.ActivitiesDBContext>(options => options.UseSqlServer(config.GetConnectionString("ActivitiesBot")));

services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(log, dispose: true));
services.AddSingleton(new DiscordSocketClient(
    new DiscordSocketConfig
    {
        FormatUsersInBidirectionalUnicode = false,
        LogGatewayIntentWarnings = false
    }));
services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
services.AddSingleton<InteractionHandler>();

services.AddTransient<LangProvider>();
services.AddSingleton<Activities_Bot.Activities_Bot>();

services.AddLocalization(options => options.ResourcesPath = "Resources");

var provider = services.BuildServiceProvider();

var prov = provider.GetRequiredService<Activities_Bot.Database.ActivitiesDBContext>();
prov.Database.EnsureCreated();

await provider.GetRequiredService<Activities_Bot.Activities_Bot>().StartAsync();