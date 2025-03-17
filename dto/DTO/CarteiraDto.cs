
namespace WebApplication5.DTO
{
    public class CarteiraDto
    {
        public Guid Id { get; set; }
        
        public Guid? UtilizadorId { get; set; }
        
        public Guid? AtivoId { get; set; }
        
        public DateTime DataInicio { get; set; } // Usando DateTime ao invés de DateOnly
        
        public double Montante { get; set; }
        
        // Propriedades de navegação (pode ser útil dependendo da necessidade)
        public string? UtilizadorNome { get; set; }
        public string? AtivoTipo { get; set; }
    }
}