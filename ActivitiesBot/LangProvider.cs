using System.Diagnostics.CodeAnalysis;

namespace ActivitiesBot;

public class LangProvider(IStringLocalizer<LangProvider> localizer)
{
    [return: NotNullIfNotNull("localizer")]
    public string? GetString(string key, string? culture = null)
    {
        if (culture is null)
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
        else
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        return localizer[key]; 
    }


    [return: NotNullIfNotNull("localizer")]
    public string? GetString(string key, CultureInfo cultureInfo)
    {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        return localizer[key];
    }

}
