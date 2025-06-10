namespace WebApplication5.DTO;

public class UtilizadorDashboardDto
{
    public List<CarteiraDto> Carteiras { get; set; }
    public List<AtivosFinanceiroDto> AtivosFinanceiros { get; set; }
    public List<ImoveisArrendamentoDto> ImoveisArrendamento { get; set; }
    public List<LocalizacoDto> Localizacoes { get; set; }
}