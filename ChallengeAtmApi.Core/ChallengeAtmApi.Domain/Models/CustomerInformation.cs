using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Domain.Models;

public partial class CustomerInformation
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public double AccountBalance { get; set; }

    public virtual ICollection<CardInformation> CardInformations { get; set; } = new List<CardInformation>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
