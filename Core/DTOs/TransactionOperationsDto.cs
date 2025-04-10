using ChallengeAtmApi.Core.Models;
using System.Text.Json.Serialization;

namespace ChallengeAtmApi.Core.DTOs
{
    public class TransactionOperationsDto
    {
        //Indico explicitamente el tipo object <t> a Operations para flexibilidad, ningun otro metodo depende de esta respuesta hoy.
        public List<object> Operations { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
