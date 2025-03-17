namespace WebApplication5.DTO;

public class UtilizadoreDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    
    public string Senha { get; set; } 

    public string Cargo { get; set; }
    public Guid? PermissaoId { get; set; }
    
}