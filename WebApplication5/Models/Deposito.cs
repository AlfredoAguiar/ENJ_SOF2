using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Deposito
{
    public Guid Id { get; set; }

    public Guid? AtivoId { get; set; }

    public double Montante { get; set; }

    public string Banco { get; set; } = null!;

    public string NumeroConta { get; set; } = null!;

    public double TaxaJuros { get; set; }

    public List<string> Titulares { get; set; } = null!;

    public virtual AtivosFinanceiro? Ativo { get; set; }
}
