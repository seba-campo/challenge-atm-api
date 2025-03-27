using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class Auth
{
    public long Id { get; set; }

    public Guid? CustomerId { get; set; }

    public string? HashedPin { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CustomerInformation? Customer { get; set; }

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
