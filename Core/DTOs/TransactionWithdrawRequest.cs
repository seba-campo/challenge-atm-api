namespace ChallengeAtmApi.Core.DTOs
{
    public class TransactionWithdrawRequest
    {
        public int cardNumber { get; set; }
        public float amount { get; set; }
    }
}
