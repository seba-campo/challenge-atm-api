namespace ChallengeAtmApi.Core.DTOs
{
    public class TransactionWithdrawDto
    {
        public Guid account { get; set; }
        public double amountOfWithdrawal { get; set; }
        public double balance { get; set; }
    }
}
