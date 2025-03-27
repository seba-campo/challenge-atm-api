using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class Token
{
    public long Id { get; set; }

    public string? Token1 { get; set; }

    public long? AuthId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Auth? Auth { get; set; }
}
