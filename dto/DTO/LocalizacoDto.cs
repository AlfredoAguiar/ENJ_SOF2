namespace WebApplication5.DTO;

public class LocalizacoDto
{
    
    public Guid Id { get; set; }
    public string Endereco { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Pais { get; set; } = null!;
    public string? CodigoPostal { get; set; }

}