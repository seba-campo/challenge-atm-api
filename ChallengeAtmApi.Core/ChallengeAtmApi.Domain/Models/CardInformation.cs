﻿using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Domain.Models;

public partial class CardInformation
{
    public int CardNumber { get; set; }

    public Guid CustomerId { get; set; }

    public bool IsBlocked { get; set; }

    public virtual ICollection<Auth> Auths { get; set; } = new List<Auth>();

    public virtual CustomerInformation Customer { get; set; } = null!;

    public virtual ICollection<FailedLoginAttempt> FailedLoginAttempts { get; set; } = new List<FailedLoginAttempt>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
