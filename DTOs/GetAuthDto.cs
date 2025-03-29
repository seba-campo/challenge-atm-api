namespace ChallengeAtmApi.DTOs
{
    public class GetAuthDto
    {
        public string hashedPin { get; set; }
        public int cardNumber { get; set; }
    }
}
