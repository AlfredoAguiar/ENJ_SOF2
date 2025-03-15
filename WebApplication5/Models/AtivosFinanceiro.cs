using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class AtivosFinanceiro
{
    public Guid Id { get; set; }

    public string Tipo { get; set; } = null!;

    public DateOnly DataInicio { get; set; }

    public double TaxaPercentagem { get; set; }

    public virtual ICollection<Carteira> Carteiras { get; set; } = new List<Carteira>();

    public virtual ICollection<Deposito> Depositos { get; set; } = new List<Deposito>();

    public virtual ICollection<FundosInvestimento> FundosInvestimentos { get; set; } = new List<FundosInvestimento>();

    public virtual ICollection<ImoveisArrendamento> ImoveisArrendamentos { get; set; } = new List<ImoveisArrendamento>();
}
