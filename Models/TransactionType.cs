using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class TransactionType
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
