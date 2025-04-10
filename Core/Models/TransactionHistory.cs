using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Core.Models;

public partial class TransactionHistory
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid TransactionTypeId { get; set; }

    public double TransactionAmount { get; set; }

    public double RemainingBalance { get; set; }

    public DateOnly TransactionDateTime { get; set; }

    public int? CardNumber { get; set; }

    public virtual CardInformation? CardNumberNavigation { get; set; }

    public virtual CustomerInformation Customer { get; set; } = null!;

    public virtual TransactionType TransactionType { get; set; } = null!;
}
