using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Localizaco
{
    public Guid Id { get; set; }

    public string Endereco { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string? CodigoPostal { get; set; }

    public virtual ICollection<ImoveisArrendamento> ImoveisArrendamentos { get; set; } = new List<ImoveisArrendamento>();
}
