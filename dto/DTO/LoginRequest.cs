namespace WebApplication5.DTO;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    
    public string Cargo { get; set; }
    
    public string Nome { get; set; }
}