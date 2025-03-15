using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Utilizadore
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public Guid? PermissaoId { get; set; }

    public virtual ICollection<Carteira> Carteiras { get; set; } = new List<Carteira>();

    public virtual Permisso? Permissao { get; set; }
}
