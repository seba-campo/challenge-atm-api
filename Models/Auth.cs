using System;
using System.Collections.Generic;

namespace ChallengeAtmApi.Models;

public partial class Auth
{
    public Guid Id { get; set; }

    public int CardNumber { get; set; }

    public string HashedPin { get; set; } = null!;

    public virtual CardInformation CardNumberNavigation { get; set; } = null!;

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
