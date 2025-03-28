﻿using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class CustomerInformation
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public double? AccountBalance { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CardInformation> CardInformations { get; set; } = new List<CardInformation>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
