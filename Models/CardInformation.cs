using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class CardInformation
{
    public long Id { get; set; }

    public Guid? CustomerId { get; set; }

    public int? CardNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CustomerInformation? Customer { get; set; }
}
