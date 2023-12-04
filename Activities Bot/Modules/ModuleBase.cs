using Activities_Bot.Database;

namespace Activities_Bot.Modules;

public abstract class ModuleBase : InteractionModuleBase<SocketInteractionContext>
{
    public InteractionService Commands { get; set; }

    public ILogger<ModuleBase> _logger { get; set; }

    public LangProvider _langProvider { get; set; }

    public ActivitiesDBContext _db { get; set; }

    public IConfiguration _config { get; set; }

    public AllActivities _activities { get; set; }

}
