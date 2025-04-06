namespace ChallengeAtmApi.DTOs
{
    public class TransactionDepositDto
    {
        public Guid account {  get; set; }
        public double ammountOfDeposit { get; set; }
        public double balance {  get; set; }
        
    }
}
