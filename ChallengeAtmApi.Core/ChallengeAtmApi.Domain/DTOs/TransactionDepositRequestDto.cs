namespace ChallengeAtmApi.Domain.DTOs
{
    public class TransactionDepositRequest
    {
        public double ammountOfDeposit { get; set; }
        public int cardNumber { get; set; }
    }
}
