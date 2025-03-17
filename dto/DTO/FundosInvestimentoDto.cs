namespace WebApplication5.DTO
{
    public class FundosInvestimentoDto
    {
        public Guid Id { get; set; }
        public Guid? AtivoId { get; set; } 
        public string Nome { get; set; } = null!;
        public double MontanteInvestido { get; set; }
        public int DuracaoMeses { get; set; }
        public double TaxaJurosPadrao { get; set; }
    }
}
