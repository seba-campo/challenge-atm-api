using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class Token
{
    public Guid Id { get; set; }

    public Guid Token1 { get; set; }

    public Guid AuthId { get; set; }

    public virtual Auth Auth { get; set; } = null!;
}
