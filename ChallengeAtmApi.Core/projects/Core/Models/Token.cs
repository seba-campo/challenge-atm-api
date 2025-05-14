using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Core.Models;

public partial class Token
{
    public Guid Id { get; set; }

    public string Token1 { get; set; } = null!;

    public Guid AuthId { get; set; }

    public virtual Auth Auth { get; set; } = null!;
}
