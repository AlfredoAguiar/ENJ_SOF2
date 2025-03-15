using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class FundosInvestimento
{
    public Guid Id { get; set; }

    public Guid? AtivoId { get; set; }

    public string Nome { get; set; } = null!;

    public double MontanteInvestido { get; set; }

    public int DuracaoMeses { get; set; }

    public double TaxaJurosPadrao { get; set; }

    public virtual AtivosFinanceiro? Ativo { get; set; }
}
