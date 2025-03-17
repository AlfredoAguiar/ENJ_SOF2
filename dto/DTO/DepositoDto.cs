namespace WebApplication5.DTO
{
    public class DepositoDto
    {
        public Guid Id { get; set; }
        public Guid? AtivoId { get; set; }
        public double Montante { get; set; }
        public string Banco { get; set; } = null!;
        public string NumeroConta { get; set; } = null!;
        public double TaxaJuros { get; set; }
        public List<string> Titulares { get; set; } = null!;

        // Propriedade adicional para exibir o tipo do ativo, caso necessário
        public string? AtivoTipo { get; set; }
    }
}