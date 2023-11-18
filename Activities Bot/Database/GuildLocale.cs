using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Activities_Bot.Database;

[Table("GuildLocales")]
public class GuildLocale
{
    [Column("GuildId")]
    [Key]
    public ulong GuildId { get; set; }

    [Column("Culture")]
    [DefaultValue("en-US")]
    [System.ComponentModel.DataAnnotations.MaxLength(10)]
    public string Culture { get; set; }
}
