using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Core.Models;

public partial class TransactionType
{
    public Guid Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
