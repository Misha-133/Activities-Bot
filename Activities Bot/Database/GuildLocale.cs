using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Activities_Bot.Database;

[Table("GuildLocales")]
public class GuildLocale
{
    [Column("GuildId")]
    [Key]
    public ulong GuildId { get; set; }

    [Column("Culture")]
    [DefaultValue("en-US")]
    [MaxLength(10)]
    public string Culture { get; set; }
}
