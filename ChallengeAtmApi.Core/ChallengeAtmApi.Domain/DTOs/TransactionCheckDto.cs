namespace ChallengeAtmApi.Domain.DTOs
{
    public class TransactionCheckDto
    {
        public string nombre { get; set; }
        public Guid accountId { get; set; }
        public double balance { get; set; }
        public DateOnly lastTransaction { get; set; }
        //var data = new
        //{
        //    nombre = customerInformation.UserName,
        //    accoutId = customerInformation.Id,
        //    balance = customerInformation.AccountBalance,
        //    lastTransaction = customerInformation.TransactionHistories.LastOrDefault()
        //};
    }
}
