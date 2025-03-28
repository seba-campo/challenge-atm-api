using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class Auth
{
    public long Id { get; set; }

    public string? HashedPin { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CardNumber { get; set; }

    public virtual CardInformation? CardNumberNavigation { get; set; }

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
