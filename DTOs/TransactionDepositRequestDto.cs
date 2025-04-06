namespace ChallengeAtmApi.DTOs
{
    public class TransactionDepositRequest
    {
        public Guid account { get; set; }
        public double ammountOfDeposit { get; set; }
    }
}
