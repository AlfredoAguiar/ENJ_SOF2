namespace WebApplication5.DTO
{
    public class AtivosFinanceiroDto
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = null!;
        public DateOnly DataInicio { get; set; }
        public double TaxaPercentagem { get; set; }
    }
}