using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class CardInformation
{
    public long Id { get; set; }

    public Guid? CustomerId { get; set; }

    public int CardNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? IsBlocked { get; set; }

    public virtual ICollection<Auth> Auths { get; set; } = new List<Auth>();

    public virtual CustomerInformation? Customer { get; set; }

    public virtual ICollection<FailedLoginAttempt> FailedLoginAttempts { get; set; } = new List<FailedLoginAttempt>();
}
