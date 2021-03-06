using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activities_Bot;

public class LangProvider
{
    private readonly IStringLocalizer<LangProvider> _localizer = null!;

    public LangProvider(IStringLocalizer<LangProvider> localizer) 
        => _localizer = localizer;

    [return: NotNullIfNotNull("_localizer")]
    public string? GetString(string key, string? culture = null)
    {
        if (culture is null)
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
        else
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        return _localizer[key]; 
    }


    [return: NotNullIfNotNull("_localizer")]
    public string? GetString(string key, CultureInfo cultureInfo)
    {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        return _localizer[key];
    }

}
