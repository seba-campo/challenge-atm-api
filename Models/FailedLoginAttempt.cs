using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class FailedLoginAttempt
{
    public Guid Id { get; set; }

    public int CardNumber { get; set; }

    public int AttemptCount { get; set; }

    public DateTime LastAttempt { get; set; }

    public virtual CardInformation CardNumberNavigation { get; set; } = null!;
}
