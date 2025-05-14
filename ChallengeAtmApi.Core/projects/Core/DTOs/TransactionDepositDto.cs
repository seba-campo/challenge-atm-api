namespace ChallengeAtmApi.projects.Core.DTOs
{
    public class TransactionDepositDto
    {
        public Guid account { get; set; }
        public double amountOfDeposit { get; set; }
        public double balance { get; set; }

    }
}
