namespace WebApplication5.DTO

{
    public class ImoveisArrendamentoDto
    {
        public Guid Id { get; set; }
        public Guid? AtivoId { get; set; }
        public string Designacao { get; set; } = null!;
        public Guid? LocalizacaoId { get; set; }
        public double ValorPropriedade { get; set; }
        public double ValorRenda { get; set; }
        public double TaxaCondominio { get; set; }
        public double DespesasAnuais { get; set; }
        public double PercentagemPropriedade { get; set; }

        // Propriedades adicionais (caso queira incluir dados de Ativo e Localizacao)
        public string? AtivoTipo { get; set; }
        public string? LocalizacaoCidade { get; set; }
    }
}
