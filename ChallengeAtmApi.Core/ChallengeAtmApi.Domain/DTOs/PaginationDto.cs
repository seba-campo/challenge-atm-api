namespace ChallengeAtmApi.Domain.DTOs
{
    public class PaginationDto
    {
        public int totalOperations { get; set; }
        public int totalPages { get; set; }
        public int actualPage { get; set; }
        public int paginationSize { get; set; }
    }
}
